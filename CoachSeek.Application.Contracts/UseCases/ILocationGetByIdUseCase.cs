using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ILocationGetByIdUseCase
    {
        Guid BusinessId { get; set; }

        LocationData GetLocation(Guid id);
    }
}
