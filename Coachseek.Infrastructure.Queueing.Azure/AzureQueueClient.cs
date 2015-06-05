using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Coachseek.Infrastructure.Queueing.Contracts;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Coachseek.Infrastructure.Queueing.Azure
{
    public class AzureQueueClient : IQueueClient<CloudQueueMessage>
    {
        private CloudQueue _cloudQueue;

        private string QueueName { get; set; }
        private CloudQueueClient QueueClient { get; set; }

        protected virtual string ConnectionStringKey { get { return "StorageConnectionString"; } }

        protected CloudStorageAccount StorageAccount
        {
            get
            {
                var connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringKey].ConnectionString;
                return CloudStorageAccount.Parse(connectionString);
            }
        }

        private CloudQueue CloudQueue
        {
            get
            {
                if (_cloudQueue == null)
                {
                    QueueClient = StorageAccount.CreateCloudQueueClient();
                    var cloudQueue = QueueClient.GetQueueReference(QueueName);
                    cloudQueue.CreateIfNotExists();
                    _cloudQueue = cloudQueue;
                }

                return _cloudQueue;
            }
        }


        public Queue GetQueue(string queueName)
        {
            QueueName = queueName;
            return new Queue(QueueName, CloudQueue.Uri.AbsoluteUri);
        }

        public void Push(Queue queue, CloudQueueMessage message)
        {
            CloudQueue.AddMessage(message);
        }

        public IList<CloudQueueMessage> Peek(Queue queue, int maxCount = 10)
        {
            return CloudQueue.GetMessages(maxCount).ToList();
        }

        public void Pop(Queue queue, CloudQueueMessage message)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }
    }
}
