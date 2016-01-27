using System;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.Contracts.UseCases.Executors;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Application.UseCases.Executors
{
    public class CustomFieldUseCaseExecutor : ICustomFieldUseCaseExecutor
    {
        public ICustomFieldTemplateSetIsActiveUseCase CustomFieldTemplateSetIsActiveUseCase { get; set; }

        public CustomFieldUseCaseExecutor(ICustomFieldTemplateSetIsActiveUseCase customFieldTemplateSetIsActiveUseCase)
        {
            CustomFieldTemplateSetIsActiveUseCase = customFieldTemplateSetIsActiveUseCase;
        }


        public async Task<IResponse> ExecuteForAsync<T>(T command, ApplicationContext context) where T : ICommand
        {
            if (command.GetType() == typeof(CustomFieldTemplateSetIsActiveCommand))
            {
                CustomFieldTemplateSetIsActiveUseCase.Initialise(context);
                return await CustomFieldTemplateSetIsActiveUseCase.SetIsActiveAsync(command as CustomFieldTemplateSetIsActiveCommand);
            }

            throw new NotImplementedException();
        }
    }
}
