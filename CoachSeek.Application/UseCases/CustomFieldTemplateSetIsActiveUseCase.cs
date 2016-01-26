using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.UseCases
{
    public class CustomFieldTemplateSetIsActiveUseCase : BaseUseCase, ICustomFieldTemplateSetIsActiveUseCase
    {
        public async Task<Response> SetIsActiveAsync(CustomFieldTemplateSetIsActiveCommand command)
        {
            var template = await BusinessRepository.GetCustomFieldTemplateAsync(Business.Id, command.TemplateId);
            if (template.IsNotFound())
                return new NotFoundResponse();
            await BusinessRepository.SetCustomFieldTemplateIsActiveAsync(Business.Id, template.Id, command.IsActive);
            return new Response();
        }
    }
}
