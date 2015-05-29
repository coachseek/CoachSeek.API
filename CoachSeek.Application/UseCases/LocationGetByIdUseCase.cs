using System;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class LocationGetByIdUseCase : BaseUseCase, ILocationGetByIdUseCase
    {
        public LocationData GetLocation(Guid id)
        {
            return BusinessRepository.GetLocation(Business.Id, id);
        }
    }
}
