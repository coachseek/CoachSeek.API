using System;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.Contracts.UseCases.Executors;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Application.UseCases.Executors
{
    public class AdminUseCaseExecutor : IAdminUseCaseExecutor
    {
        public IBusinessSetAuthorisedUntilUseCase BusinessSetAuthorisedUntilUseCase { get; set; }

        public AdminUseCaseExecutor(IBusinessSetAuthorisedUntilUseCase businessSetAuthorisedUntilUseCase)
        {
            BusinessSetAuthorisedUntilUseCase = businessSetAuthorisedUntilUseCase;
        }


        public async Task<IResponse> ExecuteForAsync<T>(T command, AdminApplicationContext context) where T : ICommand
        {
            if (command.GetType() == typeof(BusinessSetAuthorisedUntilCommand))
            {
                BusinessSetAuthorisedUntilUseCase.Initialise(context);
                return await BusinessSetAuthorisedUntilUseCase.SetAuthorisedUntilAsync(command as BusinessSetAuthorisedUntilCommand);
            }

            throw new NotImplementedException();
        }
    }
}