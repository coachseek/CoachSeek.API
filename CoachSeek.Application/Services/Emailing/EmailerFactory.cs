using Coachseek.Integration.Contracts.Interfaces;
using Coachseek.Integration.Emailing.Amazon;

namespace CoachSeek.Application.Services.Emailing
{
    public static class EmailerFactory
    {
        public static IEmailer CreateEmailer(bool isEmailingEnabled, bool isTesting, bool forceEmail)
        {
            if (!isEmailingEnabled)
                return CreateNullEmailer();

            if (isTesting && !forceEmail)
                return CreateTestingEmailer(); 

            return CreateProductionEmailer();
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

        private static IEmailer CreateProductionEmailer()
        {
            return new AmazonEmailer();
        }
    }
}