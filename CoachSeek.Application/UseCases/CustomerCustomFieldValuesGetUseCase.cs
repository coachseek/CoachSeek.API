using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class CustomerCustomFieldValuesGetUseCase : BaseUseCase, ICustomerCustomFieldValuesGetUseCase
    {
        private ICustomFieldTemplatesGetByTypeAndKeyUseCase CustomFieldTemplatesGetByTypeAndKeyUseCase { get; set; }

        public CustomerCustomFieldValuesGetUseCase(ICustomFieldTemplatesGetByTypeAndKeyUseCase customFieldTemplatesGetByTypeAndKeyUseCase)
        {
            CustomFieldTemplatesGetByTypeAndKeyUseCase = customFieldTemplatesGetByTypeAndKeyUseCase;
        }

        public async Task<IList<CustomFieldKeyValueData>> GetCustomFieldsAsync(Guid customerId)
        {
            var customFields = new List<CustomFieldKeyValueData>();

            var templates = await LookupCustomerCustomFieldTemplatesAsync();
            var values = await LookupCustomerCustomFieldValuesAsync(customerId);

            foreach (var template in templates)
            {
                // Always return even inactive template values.
                //if (!template.IsActive)
                //    continue;
                var value = values.SingleOrDefault(x => x.Key == template.Key);
                if (value.IsFound())
                    customFields.Add(value.ToKeyValue());
                else
                    customFields.Add(new CustomFieldKeyValueData(template.Key));
            }

            return customFields;
        }


        private async Task<IList<CustomFieldTemplateData>> LookupCustomerCustomFieldTemplatesAsync()
        {
            CustomFieldTemplatesGetByTypeAndKeyUseCase.Initialise(Context);
            return await CustomFieldTemplatesGetByTypeAndKeyUseCase.GetCustomFieldTemplatesByTypeAndKeyAsync(Constants.CUSTOM_FIELD_TYPE_CUSTOMER);
        }

        private async Task<IList<CustomFieldValueData>> LookupCustomerCustomFieldValuesAsync(Guid customerId)
        {
            return await BusinessRepository.GetCustomFieldValuesByTypeIdAsync(Business.Id,
                                                                              Constants.CUSTOM_FIELD_TYPE_CUSTOMER,
                                                                              customerId);
        }
    }
}
