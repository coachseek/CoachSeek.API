using Amazon;
using Amazon.SQS.Model;
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
                var listQueuesRequest = new ListQueuesRequest();
                var listQueuesResponse = sqs.ListQueues(listQueuesRequest);

                var bouncedQueueUrl = string.Empty;

                foreach (var queueUrl in listQueuesResponse.QueueUrls)
                    if (queueUrl.Contains("ses-bounces-queue"))
                        bouncedQueueUrl = queueUrl;

                var receiveMessageRequest = new ReceiveMessageRequest {QueueUrl = bouncedQueueUrl};
                var receiveMessageResponse = sqs.ReceiveMessage(receiveMessageRequest);

                ProcessQueuedBounce(receiveMessageResponse);
            }
        }


        /// <summary>Process bounces received from Amazon SES via Amazon SQS.</summary>
        /// <param name="response">The response from the Amazon SQS bounces queue 
        /// to a ReceiveMessage request. This object contains the Amazon SES  
        /// bounce notification.</param> 
        private static void ProcessQueuedBounce(ReceiveMessageResponse response)
        {
            int messages = response.Messages.Count;

            if (messages > 0)
            {
                foreach (var m in response.Messages)
                {
                    // First, convert the Amazon SNS message into a JSON object.
                    var notification = JsonConvert.DeserializeObject<AmazonSqsNotification>(m.Body);

                    // Now access the Amazon SES bounce notification.
                    var bounce = JsonConvert.DeserializeObject<AmazonSesBounceNotification>(notification.Message);

                    switch (bounce.Bounce.BounceType)
                    {
                        case "Transient":
                            // Per our sample organizational policy, we will remove all recipients 
                            // that generate an AttachmentRejected bounce from our mailing list.
                            // Other bounces will be reviewed manually.
                            switch (bounce.Bounce.BounceSubType)
                            {
                                case "AttachmentRejected":
                                    foreach (var recipient in bounce.Bounce.BouncedRecipients)
                                    {
                                        //RemoveFromMailingList(recipient.EmailAddress);
                                    }
                                    break;
                                default:
                                    //ManuallyReviewBounce(bounce);
                                    break;
                            }
                            break;
                        default:
                            // Remove all recipients that generated a permanent bounce 
                            // or an unknown bounce.
                            foreach (var recipient in bounce.Bounce.BouncedRecipients)
                            {
                                //RemoveFromMailingList(recipient.EmailAddress);
                            }
                            break;
                    }
                }
            }
        }
    }
}
