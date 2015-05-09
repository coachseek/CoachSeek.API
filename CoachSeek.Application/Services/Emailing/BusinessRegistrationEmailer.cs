using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CoachSeek.Application.Contracts.Services.Emailing;
using CoachSeek.Common.Services.Templating;
using CoachSeek.Data.Model;
using Coachseek.Integration.Contracts.Interfaces;
using Coachseek.Integration.Contracts.Models;

namespace CoachSeek.Application.Services.Emailing
{
    public class BusinessRegistrationEmailer : IBusinessRegistrationEmailer
    {
        private const string TEMPLATE_RESOURCE = "CoachSeek.Application.Services.Emailing.Templates.BusinessRegistrationEmail.txt";

        public bool IsTesting { get; set; }
        public bool ForceEmail { get; set; }
        public string Sender { get; set; }


        private IEmailer Emailer
        {
            get { return EmailerFactory.CreateEmailer(IsTesting, ForceEmail); }
        }


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
            var values = new Dictionary<string, string>();

            values.Add("BusinessName", registration.Business.Name);
            values.Add("BusinessDomain", registration.Business.Domain);
            values.Add("FirstName", registration.Admin.FirstName);
            values.Add("LastName", registration.Admin.LastName);
            values.Add("UserName", registration.Admin.Username);

            return values;
        }

        private string ReadEmbeddedTextResource(string resourceName)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                using (var reader = new StreamReader(stream))
                    return reader.ReadToEnd();
        }
    }
}
