using System.Threading.Tasks;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Application.Contracts.Services
{
    public interface ICustomerResolver : IApplicationContextSetter
    {
        Task<CustomerData> ResolveAsync(Customer customer);
    }
}
