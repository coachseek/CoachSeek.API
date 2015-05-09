using Coachseek.Integration.Contracts.Interfaces;
using Coachseek.Integration.Emailing.Amazon;

namespace CoachSeek.Application.Services.Emailing
{
    public static class EmailerFactory
    {
        public static IEmailer CreateEmailer(bool isTesting, bool forceEmail)
        {
            if (isTesting && !forceEmail)
                return CreateTestingEmailer(); 
            
            return CreateProductionEmailer();
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