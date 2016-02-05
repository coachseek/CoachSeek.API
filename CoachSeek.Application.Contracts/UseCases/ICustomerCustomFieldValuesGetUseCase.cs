using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICustomerCustomFieldValuesGetUseCase : IApplicationContextSetter
    {
        Task<IList<CustomFieldKeyValueData>> GetCustomFieldsAsync(Guid customerId);
    }
}
