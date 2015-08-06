using System.Globalization;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities.EmailTemplating;
using CoachSeek.Domain.Factories;

namespace CoachSeek.Application.UseCases
{
    public class EmailTemplateGetByTypeUseCase : BaseUseCase, IEmailTemplateGetByTypeUseCase
    {
        public EmailTemplateData GetEmailTemplate(string templateType)
        {
            if(!IsValidEmailTemplate(templateType))
                return null;
            var customisedTemplate = BusinessRepository.GetEmailTemplate(Business.Id, templateType);
            EmailTemplate template;
            if (customisedTemplate.IsExisting())
                template = EmailTemplateFactory.BuildEmailTemplate(customisedTemplate);
            else
                template = EmailTemplateFactory.BuildDefaultEmailTemplate(templateType);
            return template.ToData();
        }

        private bool IsValidEmailTemplate(string templateType)
        {
            return (templateType == Constants.EMAIL_TEMPLATE_CUSTOMER_SESSION_BOOKING ||
                    templateType == Constants.EMAIL_TEMPLATE_CUSTOMER_COURSE_BOOKING);
        }
    }
}
