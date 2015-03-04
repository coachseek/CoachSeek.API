using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ILocationGetByIdUseCase : IBusinessRepositorySetter
    {
        LocationData GetLocation(Guid id);
    }
}
