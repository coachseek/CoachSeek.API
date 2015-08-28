using System.Collections.Generic;
using System.Linq;
using Amazon.SQS.Model;
using Coachseek.API.Client.Services;
using Coachseek.Infrastructure.Queueing.Amazon.Models;
using Coachseek.Infrastructure.Queueing.Contracts;
using Coachseek.Infrastructure.Queueing.Contracts.Emailing;

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


        public IList<BouncedEmailMessage> Peek()
        {
            var sqsMessages = AmazonQueueClient.Peek(Queue);
            return ConvertToBouncedEmailMessages(sqsMessages);
        }

        public void Pop(BouncedEmailMessage message)
        {
            AmazonQueueClient.Pop(Queue, ConvertToAmazonMessage(message));
        }

        public void Dispose()
        {
            AmazonQueueClient.Dispose();
        }


        private Message ConvertToAmazonMessage(BouncedEmailMessage bouncedMessage)
        {
            return new Message {ReceiptHandle = bouncedMessage.Id};
        }

        private IList<BouncedEmailMessage> ConvertToBouncedEmailMessages(IEnumerable<Message> messages)
        {
            return messages.Select(ConvertToBouncedEmailMessage).ToList();
        }

        private BouncedEmailMessage ConvertToBouncedEmailMessage(Message message)
        {
            var notification = JsonSerialiser.Deserialise<AmazonSqsNotification>(message.Body);
            var sesBounceMessage = JsonSerialiser.Deserialise<AmazonSesBounceNotification>(notification.Message);

            var receiptId = message.ReceiptHandle;
            var bounceType = sesBounceMessage.Bounce.BounceType;
            var recipients = sesBounceMessage.Bounce.BouncedRecipients.Select(x => x.EmailAddress);

            return new BouncedEmailMessage(receiptId, bounceType, recipients);
        }
    }
}
