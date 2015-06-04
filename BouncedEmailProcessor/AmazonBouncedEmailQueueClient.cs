using System.Collections.Generic;
using System.Linq;
using Amazon.SQS.Model;
using Newtonsoft.Json;

namespace BouncedEmailProcessor
{
    public class AmazonBouncedEmailQueueClient : AmazonQueueClient<BouncedEmailMessage>, IBouncedEmailQueueClient
    {
        public Queue GetBouncedEmailQueue()
        {
            return GetQueue("ses-bounces-queue");
        }


        public IList<BouncedEmailMessage> GetBouncedEmailMessages(Queue queue)
        {
            return GetMessages(queue);
        }


        protected override BouncedEmailMessage ConvertToOutputMessage(Message message)
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
