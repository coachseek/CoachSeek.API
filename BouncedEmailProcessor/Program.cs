using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Coachseek.API.Client;
using Newtonsoft.Json;

namespace BouncedEmailProcessor
{
    /// <summary>
    /// Prototype bounced email processor.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            using (var sqs = AWSClientFactory.CreateAmazonSQSClient(RegionEndpoint.USWest2))
            {
                var bouncedQueue = GetBouncedQueue(sqs);
                var haveBouncedMessages = true;
                while (haveBouncedMessages)
                {
                    var bounceMessages = GetBouncedMessages(bouncedQueue, sqs);
                    haveBouncedMessages = bounceMessages.Count > 0;
                    ProcessQueuedBounce(bounceMessages, bouncedQueue, sqs);
                }
            }
        }


        private static BouncedQueue GetBouncedQueue(IAmazonSQS sqs)
        {
            var listQueuesRequest = new ListQueuesRequest();
            var listQueuesResponse = sqs.ListQueues(listQueuesRequest);

            foreach (var queueUrl in listQueuesResponse.QueueUrls)
                if (queueUrl.Contains("ses-bounces-queue"))
                    return new BouncedQueue(queueUrl);

            return null;
        }

        private static IList<BouncedMessage> GetBouncedMessages(BouncedQueue bouncedQueue, IAmazonSQS sqs)
        {
            if (bouncedQueue == null)
                return new List<BouncedMessage>();

            var request = new ReceiveMessageRequest { QueueUrl = bouncedQueue.Url };
            var response = sqs.ReceiveMessage(request);

            var messageCount = response.Messages.Count;
            if (messageCount == 0)
                return new List<BouncedMessage>();

            var bouncedMessages = new List<BouncedMessage>();

            foreach (var sesBounceMessage in response.Messages)
            {
                var bouncedMessage = ConvertToBouncedMessage(sesBounceMessage);
                bouncedMessages.Add(bouncedMessage);
            }

            return bouncedMessages;
        }

        private static BouncedMessage ConvertToBouncedMessage(Message message)
        {
            var notification = JsonConvert.DeserializeObject<AmazonSqsNotification>(message.Body);
            var sesBounceMessage = JsonConvert.DeserializeObject<AmazonSesBounceNotification>(notification.Message);

            var receiptId = message.ReceiptHandle;
            var bounceType = sesBounceMessage.Bounce.BounceType;
            var recipients = sesBounceMessage.Bounce.BouncedRecipients.Select(x => x.EmailAddress);

            return new BouncedMessage(receiptId, bounceType, recipients);
        }

        private static void ProcessQueuedBounce(IEnumerable<BouncedMessage> bouncedMessages, BouncedQueue bouncedQueue, IAmazonSQS sqs)
        {
            const string PERMANENT = "Permanent";

            foreach (var bouncedMessage in bouncedMessages)
            {
                if (bouncedMessage.BounceType == PERMANENT)
                {
                    foreach (var recipient in bouncedMessage.Recipients)
                        UnsubscribeEmailAddress(recipient);
                }

                PopBouncedMessageFromQueue(bouncedMessage, bouncedQueue, sqs);
            }
        }

        private static void UnsubscribeEmailAddress(string emailAddress)
        {
            var relativeUrl = string.Format("Email/Unsubscribe?email={0}", HttpUtility.UrlEncode(emailAddress));
            var response = ApiClient.AdminAuthenticatedGet<string>(relativeUrl);
            // 
        }

        private static void PopBouncedMessageFromQueue(BouncedMessage message, BouncedQueue queue, IAmazonSQS sqs)
        {
            var deleteRequest = new DeleteMessageRequest {QueueUrl = queue.Url, ReceiptHandle = message.ReceiptId};
            sqs.DeleteMessage(deleteRequest);
        }


        private class BouncedQueue
        {
            public string Url { get; private set; }

            public BouncedQueue(string url)
            {
                Url = url;
            }
        }

        private class BouncedMessage
        {
            public string ReceiptId { get; private set; }
            public string BounceType { get; private set; }
            public IEnumerable<string> Recipients { get; private set; }

            public BouncedMessage(string receiptId, string bounceType, IEnumerable<string> recipients)
            {
                BounceType = bounceType;
                ReceiptId = receiptId;
                Recipients = recipients;
            }
        }
    }
}
