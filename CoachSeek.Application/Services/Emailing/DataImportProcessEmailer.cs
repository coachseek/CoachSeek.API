using System;
using System.Text;
using CoachSeek.Application.Contracts.Services.Emailing;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Application.Services.Emailing
{
    public class DataImportProcessEmailer : BusinessEmailerBase, IDataImportProcessEmailer
    {
        public void SendProcessingSuccessfulEmail()
        {
            const string subject = "Data import successful";
            var body = CreateDataImportSuccessfulEmailBody();
            var email = new Email(Sender, AdminEmail, subject, body);
            try
            {
                Emailer.Send(email);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public void SendProcessingWithErrorsEmail()
        {
            throw new System.NotImplementedException();
        }


        private string CreateDataImportSuccessfulEmailBody()
        {
            var builder = new StringBuilder();

            builder.AppendLine("Hi,");
            builder.AppendLine();
            builder.AppendLine("Your customers were successfully imported into Coachseek.");
            builder.AppendLine();
            builder.AppendLine("The Coachseek Team.");

            return builder.ToString();
        }
    }
}
