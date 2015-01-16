using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ILocationGetUseCase
    {
        Guid BusinessId { get; set; }

        LocationData GetLocation(Guid id);
    }
}
