using System.Threading.Tasks;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Application.Contracts.Services
{
    public interface ICustomerResolver : IApplicationContextSetter
    {
        Task<Customer> ResolveAsync(Customer customer);
    }
}
