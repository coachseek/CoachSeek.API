using System.Collections.Generic;
using Coachseek.API.Client.Interfaces;
using CoachSeek.Application.Contracts.UseCases.Emailing;
using Coachseek.Infrastructure.Queueing.Contracts.Emailing;

namespace CoachSeek.Application.UseCases.Emailing
{
    public class ProcessBouncedEmailMessagesUseCase : IProcessBouncedEmailMessagesUseCase
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
                var haveBouncedMessages = true;
                while (haveBouncedMessages)
                {
                    var bouncedMessages = QueueClient.Peek();
                    haveBouncedMessages = bouncedMessages.Count > 0;
                    ProcessBouncedEmailMessages(bouncedMessages);
                }
            }
            finally
            {
                QueueClient.Dispose();
            }
        }


        private void ProcessBouncedEmailMessages(IEnumerable<BouncedEmailMessage> bouncedMessages)
        {
            const string PERMANENT = "Permanent";

            foreach (var bouncedMessage in bouncedMessages)
            {
                if (bouncedMessage.BounceType == PERMANENT)
                    foreach (var recipient in bouncedMessage.Recipients)
                        ApiClient.UnsubscribeEmailAddress(recipient);

                QueueClient.Pop(bouncedMessage);
            }
        }
    }
}
