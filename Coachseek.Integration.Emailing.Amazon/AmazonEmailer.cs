using System;
using System.Collections.Generic;
using System.Configuration;
using Amazon;
using Amazon.SimpleEmail.Model;
using Coachseek.Integration.Contracts.Interfaces;
using Coachseek.Integration.Contracts.Models;

namespace Coachseek.Integration.Emailing.Amazon
{
    public class AmazonEmailer : IEmailer
    {
        public void Send(Email email)
        {
            // TODO: Remove the overriding of the email addresses.
            email.Recipient = "olaft@ihug.co.nz";


            var destination = new Destination { ToAddresses = (new List<string> { email.Recipient }) };
            var subject = new Content(email.Subject);
            var body = new Body(new Content(email.Body));
            var message = new Message(subject, body);
            var request = new SendEmailRequest(email.Sender, destination, message);

            using (var client = AWSClientFactory.CreateAmazonSimpleEmailServiceClient())
            {
                try
                {
                    var response = client.SendEmail(request);
                }
                catch (MessageRejectedException ex)
                {
                    // TODO: Log error.
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
