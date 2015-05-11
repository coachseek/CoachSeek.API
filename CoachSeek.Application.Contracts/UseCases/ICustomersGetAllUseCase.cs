using System.Collections.Generic;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICustomersGetAllUseCase : IApplicationContextSetter
    {
        IList<CustomerData> GetCustomers();
    }
}
