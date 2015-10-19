using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
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

        private CloudQueue CreateCloudQueue()
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

        private async Task<CloudQueue> CreateCloudQueueAsync()
        {
            if (_cloudQueue == null)
            {
                QueueClient = StorageAccount.CreateCloudQueueClient();
                var cloudQueue = QueueClient.GetQueueReference(QueueName);
                await cloudQueue.CreateIfNotExistsAsync();
                _cloudQueue = cloudQueue;
            }

            return _cloudQueue;
        }

        public async Task<Queue> GetQueueAsync(string queueName)
        {
            QueueName = queueName;
            var cloudQueue = await CreateCloudQueueAsync();
            return new Queue(QueueName, cloudQueue.Uri.AbsoluteUri);
        }

        public Queue GetQueue(string queueName)
        {
            QueueName = queueName;
            return new Queue(QueueName, CreateCloudQueue().Uri.AbsoluteUri);
        }


        public async Task PushAsync(Queue queue, CloudQueueMessage message)
        {
            var cloudQueue = await CreateCloudQueueAsync();
            await cloudQueue.AddMessageAsync(message);
        }

        public void Push(Queue queue, CloudQueueMessage message)
        {
            CreateCloudQueue().AddMessage(message);
        }

        public async Task<IList<CloudQueueMessage>> PeekAsync(Queue queue, int maxCount = 10)
        {
            var cloudQueue = await CreateCloudQueueAsync();
            var messages = await cloudQueue.GetMessagesAsync(maxCount);
            return messages.ToList();
        }

        public IList<CloudQueueMessage> Peek(Queue queue, int maxCount = 10)
        {
            return CreateCloudQueue().GetMessages(maxCount).ToList();
        }

        public async Task PopAsync(Queue queue, CloudQueueMessage message)
        {
            var cloudQueue = await CreateCloudQueueAsync();
            await cloudQueue.DeleteMessageAsync(message.Id, message.PopReceipt);
        }

        public void Pop(Queue queue, CloudQueueMessage message)
        {
            CreateCloudQueue().DeleteMessage(message.Id, message.PopReceipt);
        }

        public void Dispose()
        { }
    }
}
