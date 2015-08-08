using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Factories;

namespace CoachSeek.Application.UseCases
{
    public class EmailTemplateGetByTypeUseCase : BaseUseCase, IEmailTemplateGetByTypeUseCase
    {
        public EmailTemplateData GetEmailTemplate(string templateType)
        {
            if(!IsValidEmailTemplate(templateType))
                return null;
            var customisedTemplate = GetCustomisedEmailTemplate(templateType);
            if (customisedTemplate.IsExisting())
                return ConstructCustomisedEmailTemplateData(customisedTemplate);
            return ConstructDefaultEmailTemplateData(templateType);
        }

        private bool IsValidEmailTemplate(string templateType)
        {
            return EmailTemplateFactory.IsValidTemplateType(templateType);
        }

        private EmailTemplateData GetCustomisedEmailTemplate(string templateType)
        {
            return BusinessRepository.GetEmailTemplate(Business.Id, templateType);
        }

        private EmailTemplateData ConstructCustomisedEmailTemplateData(EmailTemplateData customisedTemplate)
        {
            // The purpose of this code is to populate the Placeholders property.
            return EmailTemplateFactory.BuildEmailTemplate(customisedTemplate).ToData();
        }

        private EmailTemplateData ConstructDefaultEmailTemplateData(string templateType)
        {
            return EmailTemplateFactory.BuildDefaultEmailTemplate(templateType).ToData();
        }
    }
}
