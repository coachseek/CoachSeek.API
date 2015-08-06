using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Factories;

namespace CoachSeek.Application.UseCases
{
    public class EmailTemplateDeleteUseCase : BaseUseCase, IEmailTemplateDeleteUseCase
    {
        public Response DeleteEmailTemplate(string templateType)
        {
            if (!EmailTemplateFactory.IsValidTemplateType(templateType))
                return new NotFoundResponse();
            var emailTemplate = BusinessRepository.GetEmailTemplate(Business.Id, templateType);
            if (emailTemplate.IsFound())
                BusinessRepository.DeleteEmailTemplate(Business.Id, templateType);
            return new Response();
        }
    }
}