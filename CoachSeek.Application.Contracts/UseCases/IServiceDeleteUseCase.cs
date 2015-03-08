using System;
using CoachSeek.Application.Contracts.Models;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IServiceDeleteUseCase : IBusinessRepositorySetter
    {
        Response DeleteService(Guid id);
    }
}
