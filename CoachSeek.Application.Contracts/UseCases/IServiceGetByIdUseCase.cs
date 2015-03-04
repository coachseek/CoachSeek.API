using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IServiceGetByIdUseCase : IBusinessRepositorySetter
    {
        ServiceData GetService(Guid id);
    }
}
