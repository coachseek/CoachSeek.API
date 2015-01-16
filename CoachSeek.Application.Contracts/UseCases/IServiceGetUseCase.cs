using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IServiceGetUseCase
    {
        Guid BusinessId { get; set; }

        ServiceData GetService(Guid id);
    }
}
