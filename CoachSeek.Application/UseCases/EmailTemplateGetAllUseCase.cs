using System.Collections.Generic;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities.EmailTemplating;

namespace CoachSeek.Application.UseCases
{
    public class EmailTemplateGetAllUseCase : BaseUseCase, IEmailTemplateGetAllUseCase
    {
        public IList<EmailTemplateData> GetEmailTemplates()
        {
            var customisedTemplates = BusinessRepository.GetAllEmailTemplates(Business.Id);
            var templates = new EmailTemplateCollection(customisedTemplates);
            return templates.ToData();
        }
    }
}
