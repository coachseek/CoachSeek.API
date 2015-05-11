using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Application.Contracts.Services
{
    public interface ICustomerResolver : IApplicationContextSetter
    {
        Customer Resolve(Customer customer);
    }
}
