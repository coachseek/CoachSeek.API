using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class CustomersGetAllUseCase : BaseUseCase, ICustomersGetAllUseCase
    {
        public async Task<IList<CustomerData>> GetCustomersAsync()
        {
            var customers =  await BusinessRepository.GetAllCustomersAsync(Business.Id);
            var templates = await LookupCustomerCustomFieldTemplates();
            var values = await LookupCustomerCustomFieldValues();

            foreach (var customer in customers)
            {
                var customFields = new List<CustomFieldKeyValueData>();
                foreach (var template in templates)
                {
                    if (!template.IsActive)
                        continue;
                    var value = values.SingleOrDefault(x => x.Key == template.Key && x.TypeId == customer.Id);
                    if (value.IsFound())
                        customFields.Add(value.ToKeyValue());
                    else
                        customFields.Add(new CustomFieldKeyValueData(template.Key));
                }
                customer.CustomFields = customFields;
            }

            return customers;
        }

        private async Task<IList<CustomFieldTemplateData>> LookupCustomerCustomFieldTemplates()
        {
            return await BusinessRepository.GetCustomFieldTemplatesAsync(Business.Id, Constants.CUSTOM_FIELD_TYPE_CUSTOMER);
        }

        private async Task<IList<CustomFieldValueData>> LookupCustomerCustomFieldValues()
        {
            return await BusinessRepository.GetCustomFieldValuesByTypeAsync(Business.Id, Constants.CUSTOM_FIELD_TYPE_CUSTOMER);
        }
    }
}
