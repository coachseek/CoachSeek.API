using System.Collections.Generic;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities.EmailTemplating;

namespace CoachSeek.Application.UseCases
{
    public class EmailTemplateGetAllUseCase : BaseUseCase, IEmailTemplateGetAllUseCase
    {
        //private const string DELIMITER = "=====";
        //private const string TEMPLATE_RESOURCE_SESSION_CUSTOMER = "CoachSeek.Application.Services.Emailing.Templates.OnlineBookingSessionCustomerEmail.txt";
        //private const string TEMPLATE_RESOURCE_COURSE_CUSTOMER = "CoachSeek.Application.Services.Emailing.Templates.OnlineBookingCourseCustomerEmail.txt";


        public IList<EmailTemplateData> GetEmailTemplates()
        {
            var customisedTemplates = BusinessRepository.GetAllEmailTemplates(Business.Id);
            var templates = new EmailTemplateCollection(customisedTemplates);
            return templates.ToData();
        }


        //protected string ReadEmbeddedTextResource(string resourceName)
        //{
        //    using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
        //    using (var reader = new StreamReader(stream))
        //        return reader.ReadToEnd();
        //}

        //private Tuple<string, string> ExtractSubjectAndBody(string emailTemplate)
        //{
        //    var pos = emailTemplate.IndexOf(DELIMITER, StringComparison.InvariantCulture);
        //    var subject = emailTemplate.Substring(0, pos).Trim();
        //    var body = emailTemplate.Substring(pos + DELIMITER.Length).Trim();
        //    return new Tuple<string, string>(subject, body);
        //}
    }
}
