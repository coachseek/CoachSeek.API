using System.Collections.Generic;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class ServicesGetAllUseCase : BaseUseCase, IServicesGetAllUseCase
    {
        public IList<ServiceData> GetServices()
        {
            return BusinessRepository.GetAllServices(BusinessId);
        }
    }
}
