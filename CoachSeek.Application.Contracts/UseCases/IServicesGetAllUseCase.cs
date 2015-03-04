using System.Collections.Generic;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IServicesGetAllUseCase : IBusinessRepositorySetter
    {
        IList<ServiceData> GetServices();
    }
}
