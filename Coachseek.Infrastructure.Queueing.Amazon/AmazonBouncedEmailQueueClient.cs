﻿using System.Collections.Generic;
using System.Linq;
using Amazon.SQS.Model;
using Coachseek.Infrastructure.Queueing.Amazon.Models;
using Coachseek.Infrastructure.Queueing.Contracts;
using Coachseek.Infrastructure.Queueing.Contracts.Emailing;
using Newtonsoft.Json;

namespace Coachseek.Infrastructure.Queueing.Amazon
{
    public class AmazonBouncedEmailQueueClient : IBouncedEmailQueueClient
    {
        private const string QUEUE_NAME = "ses-bounces-queue";

        private AmazonQueueClient AmazonQueueClient { get; set; }


        public AmazonBouncedEmailQueueClient()
        {
            AmazonQueueClient = new AmazonQueueClient();
        }
  

        private Queue Queue
        {
            get { return AmazonQueueClient.GetQueue(QUEUE_NAME); }
        }


        public IList<BouncedEmailMessage> GetBouncedEmailMessages()
        {
            var sqsMessages = AmazonQueueClient.GetMessages(Queue);
            return ConvertToBouncedEmailMessages(sqsMessages);
        }

        public void PopBouncedEmailMessageFromQueue(BouncedEmailMessage message)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            AmazonQueueClient.Dispose();
        }


        private IList<BouncedEmailMessage> ConvertToBouncedEmailMessages(IEnumerable<Message> messages)
        {
            return messages.Select(ConvertToBouncedEmailMessage).ToList();
        }

        private BouncedEmailMessage ConvertToBouncedEmailMessage(Message message)
        {
            var notification = JsonConvert.DeserializeObject<AmazonSqsNotification>(message.Body);
            var sesBounceMessage = JsonConvert.DeserializeObject<AmazonSesBounceNotification>(notification.Message);

            var receiptId = message.ReceiptHandle;
            var bounceType = sesBounceMessage.Bounce.BounceType;
            var recipients = sesBounceMessage.Bounce.BouncedRecipients.Select(x => x.EmailAddress);

            return new BouncedEmailMessage(receiptId, bounceType, recipients);
        }
    }
}
