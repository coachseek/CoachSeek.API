using System;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class ServiceGetByIdUseCase : BaseUseCase, IServiceGetByIdUseCase
    {
        public ServiceData GetService(Guid id)
        {
            return BusinessRepository.GetService(Business.Id, id);
        }
    }
}
