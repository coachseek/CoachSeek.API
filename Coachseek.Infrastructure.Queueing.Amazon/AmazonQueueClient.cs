using System;
using System.Collections.Generic;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Coachseek.Infrastructure.Queueing.Contracts;

namespace Coachseek.Infrastructure.Queueing.Amazon
{
    public class AmazonQueueClient : IQueueClient<Message>
    {
        private IAmazonSQS SqsClient { get; set; }


        public AmazonQueueClient()
        {
            // Leave Region hard-coded for now.
            SqsClient = AWSClientFactory.CreateAmazonSQSClient(RegionEndpoint.USWest2);
        }


        public Queue GetQueue(string queueName)
        {
            var listQueuesRequest = new ListQueuesRequest();
            var listQueuesResponse = SqsClient.ListQueues(listQueuesRequest);

            foreach (var queueUrl in listQueuesResponse.QueueUrls)
                if (queueUrl.Contains(queueName))
                    return new Queue(queueName, queueUrl);

            throw new InvalidOperationException(string.Format("Queue '{0}' does not exist.", queueName));
        }


        public void PushMessageOntoQueue(Queue queue, Message message)
        {
            throw new System.NotImplementedException();
        }

        public IList<Message> GetMessages(Queue queue)
        {
            if (queue == null)
                return new List<Message>();

            var request = new ReceiveMessageRequest { QueueUrl = queue.Url };
            var response = SqsClient.ReceiveMessage(request);

            return response.Messages;
        }

        public void PopMessageFromQueue(Queue queue, Message message) 
        {
            var deleteRequest = new DeleteMessageRequest { QueueUrl = queue.Url, ReceiptHandle = message.ReceiptHandle };
            SqsClient.DeleteMessage(deleteRequest);
        }

        public void Dispose()
        {
            SqsClient.Dispose();
        }
    }
}
