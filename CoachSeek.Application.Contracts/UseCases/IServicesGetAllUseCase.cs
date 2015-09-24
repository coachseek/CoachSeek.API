using System.Collections.Generic;
using System.Threading.Tasks;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IServicesGetAllUseCase : IApplicationContextSetter
    {
        Task<IList<ServiceData>> GetServicesAsync();
    }
}
