using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.Contracts.UseCases.Executors;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Application.UseCases.Executors
{
    public class BusinessUseCaseExecutor : IBusinessUseCaseExecutor
    {
        public IBusinessSetUseProRataPricingUseCase BusinessSetUseProRataPricingUseCase { get; set; }

        public BusinessUseCaseExecutor(IBusinessSetUseProRataPricingUseCase businessSetUseProRataPricingUseCase)
        {
            BusinessSetUseProRataPricingUseCase = businessSetUseProRataPricingUseCase;
        }


        public async Task<IResponse> ExecuteForAsync<T>(T command, ApplicationContext context) where T : ICommand
        {
            if (command.GetType() == typeof(BusinessSetUseProRataPricingCommand))
            {
                BusinessSetUseProRataPricingUseCase.Initialise(context);
                return await BusinessSetUseProRataPricingUseCase.SetUseProRataPricingAsync(command as BusinessSetUseProRataPricingCommand);
            }

            throw new NotImplementedException();
        }
    }
}
