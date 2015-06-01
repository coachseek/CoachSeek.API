using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Amazon;
using Amazon.SimpleEmail.Model;
using CoachSeek.Domain.Repositories;
using Coachseek.Integration.Contracts.Interfaces;
using Coachseek.Integration.Contracts.Models;

namespace Coachseek.Integration.Emailing.Amazon
{
    public class AmazonEmailer : IEmailer
    {
        private string COPY_EMAIL = "copy@coachseek.com";

        private IUnsubscribedEmailAddressRepository UnsubscribedEmailAddressRepository { get; set; }


        public AmazonEmailer(IUnsubscribedEmailAddressRepository unsubscribedEmailAddressRepository)
        {
            UnsubscribedEmailAddressRepository = unsubscribedEmailAddressRepository;
        }


        public bool Send(Email email)
        {
            var recipientList = new List<string>();

            foreach (var recipient in email.Recipients)
            {
                var isRecipientUnsubscribed = UnsubscribedEmailAddressRepository.Get(recipient);
                if (isRecipientUnsubscribed)
                    continue;
                recipientList.Add(recipient);
            }
            if (!recipientList.Any())
                return false;

            var destination = new Destination
            {
                ToAddresses = recipientList,
                BccAddresses = (new List<string> { COPY_EMAIL }), 
            };
            var subject = new Content(email.Subject);
            var body = new Body(new Content(email.Body));
            var message = new Message(subject, body);
            var request = new SendEmailRequest(email.Sender, destination, message);
            request.ReturnPath = email.Sender;

            using (var client = AWSClientFactory.CreateAmazonSimpleEmailServiceClient())
            {
                try
                {
                    var response = client.SendEmail(request);
                    return true;
                }
                catch (MessageRejectedException ex)
                {
                    // TODO: Log error.
                    return false;
                }
            }
        }

        static bool CheckRequiredFields()
        {
            var appConfig = ConfigurationManager.AppSettings;

            if (string.IsNullOrEmpty(appConfig["AWSProfileName"]))
            {
                Console.WriteLine("AWSProfileName was not set in the App.config file.");
                return false;
            }
            //if (string.IsNullOrEmpty(senderAddress))
            //{
            //    Console.WriteLine("The variable senderAddress is not set.");
            //    return false;
            //}
            //if (string.IsNullOrEmpty(receiverAddress))
            //{
            //    Console.WriteLine("The variable receiverAddress is not set.");
            //    return false;
            //}
            return true;
        }
    }
}
