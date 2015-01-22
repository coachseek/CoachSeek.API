using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IServiceGetByIdUseCase
    {
        Guid BusinessId { get; set; }

        ServiceData GetService(Guid id);
    }
}
