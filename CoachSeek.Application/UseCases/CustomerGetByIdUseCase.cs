using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class CustomerGetByIdUseCase : BaseUseCase, ICustomerGetByIdUseCase
    {
        private ICustomerCustomFieldValuesGetUseCase CustomerCustomFieldValuesGetUseCase { get; set; }

        public CustomerGetByIdUseCase(ICustomerCustomFieldValuesGetUseCase customerCustomFieldValuesGetUseCase)
        {
            CustomerCustomFieldValuesGetUseCase = customerCustomFieldValuesGetUseCase;
        }


        public async Task<CustomerData> GetCustomerAsync(Guid id)
        {
            var customer = await BusinessRepository.GetCustomerAsync(Business.Id, id);
            if (customer.IsNotFound())
                return null;
            CustomerCustomFieldValuesGetUseCase.Initialise(Context);
            var customFields = await CustomerCustomFieldValuesGetUseCase.GetCustomFieldsAsync(id);
            customer.CustomFields = new List<CustomFieldKeyValueData>(customFields);
            return customer;
        }
    }
}
