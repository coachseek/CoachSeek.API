using System.Collections.Generic;
using System.Threading.Tasks;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICustomersGetAllUseCase : IApplicationContextSetter
    {
        Task<IList<CustomerData>> GetCustomersAsync();
    }
}
