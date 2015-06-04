using System.Collections.Generic;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace BouncedEmailProcessor
{
    public abstract class AmazonQueueClient<TMessage> : IQueueClient<TMessage> where TMessage : IMessage
    {
        private IAmazonSQS SqsClient { get; set; }

        protected AmazonQueueClient()
        {
            // Leave Region hard-coded for now.
            SqsClient = AWSClientFactory.CreateAmazonSQSClient(RegionEndpoint.USWest2);
        }


        protected abstract TMessage ConvertToOutputMessage(Message message);


        protected Queue GetQueue(string queueName)
        {
            var listQueuesRequest = new ListQueuesRequest();
            var listQueuesResponse = SqsClient.ListQueues(listQueuesRequest);

            foreach (var queueUrl in listQueuesResponse.QueueUrls)
                if (queueUrl.Contains(queueName))
                    return new Queue(queueName, queueUrl);

            return null;
        }


        public IList<TMessage> GetMessages(Queue queue)
        {
            if (queue == null)
                return new List<TMessage>();

            var request = new ReceiveMessageRequest { QueueUrl = queue.Url };
            var response = SqsClient.ReceiveMessage(request);

            var messageCount = response.Messages.Count;
            if (messageCount == 0)
                return new List<TMessage>();

            var outputMessages = new List<TMessage>();

            foreach (var message in response.Messages)
            {
                var outputMessage = ConvertToOutputMessage(message);
                outputMessages.Add(outputMessage);
            }

            return outputMessages;
        }


        public void PopMessageFromQueue(TMessage message, Queue queue)
        {
            var deleteRequest = new DeleteMessageRequest { QueueUrl = queue.Url, ReceiptHandle = message.ReceiptId };
            SqsClient.DeleteMessage(deleteRequest);
        }



        public void Dispose()
        {
            SqsClient.Dispose();
        }
    }
}
