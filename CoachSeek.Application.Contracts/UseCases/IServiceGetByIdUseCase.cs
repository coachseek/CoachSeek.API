using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IServiceGetByIdUseCase : IApplicationContextSetter
    {
        ServiceData GetService(Guid id);
    }
}
