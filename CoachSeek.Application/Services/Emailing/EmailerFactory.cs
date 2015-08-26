using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Repositories;
using Coachseek.Integration.Contracts.Emailing.Interfaces;
using Coachseek.Integration.Emailing.Amazon;

namespace CoachSeek.Application.Services.Emailing
{
    public static class EmailerFactory
    {
        public static IEmailer CreateEmailer(bool isTesting, EmailContext emailContext)
        {
            if (!emailContext.IsEmailingEnabled)
                return CreateNullEmailer();

            if (isTesting && !emailContext.ForceEmail)
                return CreateTestingEmailer();

            return CreateProductionEmailer(emailContext.UnsubscribedEmailAddressRepository);
        }


        private static IEmailer CreateNullEmailer()
        {
            return new NullEmailer();
        }

        private static IEmailer CreateTestingEmailer()
        {
            // TODO: Replace the emailer with Azure Table Storage Email Logger. 
            return new NullEmailer();
            //return new AzureTableEmailRepository();
        }

        private static IEmailer CreateProductionEmailer(IUnsubscribedEmailAddressRepository unsubscribedEmailAddressRepository)
        {
            return new AmazonEmailer(unsubscribedEmailAddressRepository);
        }
    }
}