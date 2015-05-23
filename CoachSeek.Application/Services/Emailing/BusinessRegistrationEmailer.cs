using System.Collections.Generic;
using CoachSeek.Common.Services.Templating;
using CoachSeek.Data.Model;
using Coachseek.Integration.Contracts.Models;

namespace CoachSeek.Application.Services.Emailing
{
    public class BusinessRegistrationEmailer : BusinessEmailerBase
    {
        private const string TEMPLATE_RESOURCE = "CoachSeek.Application.Services.Emailing.Templates.BusinessRegistrationEmail.txt";


        public void SendEmail(RegistrationData registration)
        {
            const string subject = "Welcome to Coachseek";
            var body = CreateEmailBody(registration);

            var email = new Email(Sender, registration.Admin.Email, subject, body);

            Emailer.Send(email);
        }

        private string CreateEmailBody(RegistrationData registration)
        {
            var bodyTemplate = ReadEmbeddedTextResource(TEMPLATE_RESOURCE);
            var substitutes = CreatePlaceholderSubstitutes(registration);
            return TemplateProcessor.ProcessTemplate(bodyTemplate, substitutes);
        }

        private Dictionary<string, string> CreatePlaceholderSubstitutes(RegistrationData registration)
        {
            var values = new Dictionary<string, string>
            {
                {"BusinessName", registration.Business.Name},
                {"BusinessDomain", registration.Business.Domain},
                {"FirstName", registration.Admin.FirstName},
                {"LastName", registration.Admin.LastName},
                {"UserName", registration.Admin.Username}
            };

            return values;
        }
    }
}
