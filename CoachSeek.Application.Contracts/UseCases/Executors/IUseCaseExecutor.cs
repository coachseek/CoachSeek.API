using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Contracts;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.UseCases.Executors
{
    public interface IUseCaseExecutor
    {
        //Response ExecuteFor<T>(T command, IBusinessRepository businessRepository, Guid? businessId) where T : ICommand;
        Response ExecuteFor<T>(T command, ApplicationContext context) where T : ICommand;
    }
}