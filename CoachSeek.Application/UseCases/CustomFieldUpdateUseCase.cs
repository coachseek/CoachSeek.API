using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.UseCases
{
    public class CustomFieldUpdateUseCase : BaseUseCase, ICustomFieldUpdateUseCase
    {
        public async Task<IResponse> UpdateCustomFieldAsync(CustomFieldUpdateCommand command)
        {
            try
            {
                var template = new CustomFieldTemplate(command);
                await ValidateUpdateAsync(template);
                await BusinessRepository.AddCustomFieldTemplateAsync(Business.Id, template);
                return new Response(template.ToData());
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }

        private async Task ValidateUpdateAsync(CustomFieldTemplate template)
        {
            var existingTemplate = await BusinessRepository.GetCustomFieldTemplateAsync(Business.Id, template.Id);
            if (existingTemplate.IsNotFound())
                throw new CustomFieldTemplateIdInvalid(template.Id);
        }
    }
}
