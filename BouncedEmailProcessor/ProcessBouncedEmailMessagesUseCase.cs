using System.Collections.Generic;

namespace BouncedEmailProcessor
{
    public class ProcessBouncedEmailMessagesUseCase
    {
        private IBouncedEmailQueueClient QueueClient { get; set; }
        private ICoachseekAdminApiClient ApiClient { get; set; }

        public ProcessBouncedEmailMessagesUseCase(IBouncedEmailQueueClient queueClient, ICoachseekAdminApiClient apiClient)
        {
            QueueClient = queueClient;
            ApiClient = apiClient;
        }

        public void Process()
        {
            try
            {
                var bouncedQueue = QueueClient.GetBouncedEmailQueue();
                var haveBouncedMessages = true;
                while (haveBouncedMessages)
                {
                    var bouncedMessages = QueueClient.GetMessages(bouncedQueue);
                    haveBouncedMessages = bouncedMessages.Count > 0;
                    ProcessBouncedEmailMessages(bouncedMessages, bouncedQueue);
                }
            }
            finally
            {
                QueueClient.Dispose();
            }
        }


        private void ProcessBouncedEmailMessages(IEnumerable<BouncedEmailMessage> bouncedMessages, Queue bouncedQueue)
        {
            const string PERMANENT = "Permanent";

            foreach (var bouncedMessage in bouncedMessages)
            {
                if (bouncedMessage.BounceType == PERMANENT)
                    foreach (var recipient in bouncedMessage.Recipients)
                        ApiClient.UnsubscribeEmailAddress(recipient);

                QueueClient.PopMessageFromQueue(bouncedMessage, bouncedQueue);
            }
        }
    }
}
