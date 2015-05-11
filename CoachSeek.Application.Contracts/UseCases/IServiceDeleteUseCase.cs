using System;
using CoachSeek.Application.Contracts.Models;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IServiceDeleteUseCase : IApplicationContextSetter
    {
        Response DeleteService(Guid id);
    }
}
