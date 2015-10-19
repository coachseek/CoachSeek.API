﻿using CoachSeek.Application.Configuration;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.DataAccess.Main.Memory.Configuration;
using StructureMap;

namespace Coachseek.Integration.Payments.PaymentsProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationAutoMapperConfigurator.Configure();
            DbAutoMapperConfigurator.Configure();
            var container = new Container(new TypeRegistry());
            var useCase = container.GetInstance<IProcessPaymentsUseCase>();
            useCase.ProcessAsync().Wait();
            //useCase.Process();
        }
    }
}
