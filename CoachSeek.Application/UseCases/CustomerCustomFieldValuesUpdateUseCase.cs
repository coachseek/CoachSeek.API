using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.UseCases
{
    public class CustomerCustomFieldValuesUpdateUseCase : BaseUseCase, ICustomerCustomFieldValuesUpdateUseCase
    {
        private ICustomerGetByIdUseCase CustomerGetByIdUseCase { get; set; }

        public CustomerCustomFieldValuesUpdateUseCase(ICustomerGetByIdUseCase customerGetByIdUseCase)
        {
            CustomerGetByIdUseCase = customerGetByIdUseCase;
        }


        public async Task<IResponse> UpdateCustomerCustomFieldValuesAsync(CustomFieldValueListUpdateCommand command)
        {
            try
            {
                var templates = await LookupCustomFieldTemplatesForTypeAsync();
                var values = await LookupCustomFieldValuesByTypeIdAsync(command);
                ValidateUpdate(command, templates);
                var cmdList = SplitIntoAddUpdateDeleteCommands(command, templates, values);
                foreach (var cmd in cmdList)
                    await ExecuteAddUpdateOrDeleteCommandAsync(cmd);
                return new Response(await GetCustomerAsync(command.TypeId));
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }

        private async Task ExecuteAddUpdateOrDeleteCommandAsync(CustomFieldValueCommand cmd)
        {
            if (cmd is AddCustomFieldValueCommand)
                await AddCustomFieldValueAsync(cmd);
            if (cmd is UpdateCustomFieldValueCommand)
                await UpdateCustomFieldValueAsync(cmd);
            if (cmd is DeleteCustomFieldValueCommand)
                await DeleteCustomFieldValueAsync(cmd);            
        }

        private async Task<CustomerData> GetCustomerAsync(Guid customerId)
        {
            CustomerGetByIdUseCase.Initialise(Context);
            return await CustomerGetByIdUseCase.GetCustomerAsync(customerId);    
        }

        private async Task<IList<CustomFieldTemplateData>> LookupCustomFieldTemplatesForTypeAsync()
        {
            return await BusinessRepository.GetCustomFieldTemplatesAsync(Business.Id, Constants.CUSTOM_FIELD_TYPE_CUSTOMER);            
        }

        private async Task<IList<CustomFieldValueData>> LookupCustomFieldValuesByTypeIdAsync(CustomFieldValueListUpdateCommand command)
        {
            return await BusinessRepository.GetCustomFieldValuesByTypeIdAsync(Business.Id, command.Type, command.TypeId);
        }

        private void ValidateUpdate(CustomFieldValueListUpdateCommand command, IList<CustomFieldTemplateData> templates)
        {
            var errors = new ValidationException();
            ValidateValueKeys(command, templates, errors);
            //ValidateRequiredValues(command, templates, errors);
            errors.ThrowIfErrors();
        }

        private void ValidateValueKeys(CustomFieldValueListUpdateCommand command, IList<CustomFieldTemplateData> templates, ValidationException errors)
        {
            foreach (var customField in command.CustomFields)
            {
                var isFound = templates.Any(template => template.Key == customField.Key);
                if (!isFound)
                    errors.Add(new CustomFieldValueKeyInvalid(Constants.CUSTOM_FIELD_TYPE_CUSTOMER, customField.Key));
            }
        }

        //private void ValidateRequiredValues(CustomFieldValueListUpdateCommand command, IList<CustomFieldTemplateData> templates, ValidationException errors)
        //{
        //    foreach (var template in templates)
        //    {
        //        var isFound = command.CustomFields.Any(customField => template.Key == customField.Key);
        //        if (!isFound && template.IsRequired && template.IsActive)
        //            errors.Add(new CustomFieldValueRequired(Constants.CUSTOM_FIELD_TYPE_CUSTOMER, template.Key));
        //    }
        //}

        private IList<CustomFieldValueCommand> SplitIntoAddUpdateDeleteCommands(CustomFieldValueListUpdateCommand command, 
                                                                                IList<CustomFieldTemplateData> templates, 
                                                                                IList<CustomFieldValueData> existingValues)
        {
            var cmdList = new List<CustomFieldValueCommand>();

            foreach (var newValue in command.CustomFields)
            {
                var isFoundInTemplates = templates.Any(x => x.Key == newValue.Key);
                var isFoundInValues = existingValues.Any(x => x.Key == newValue.Key);
                if (isFoundInTemplates)
                {
                    if (!isFoundInValues)
                        cmdList.Add(new AddCustomFieldValueCommand(command.Type, command.TypeId, newValue.Key, newValue.Value));
                    else
                        cmdList.Add(new UpdateCustomFieldValueCommand(command.Type, command.TypeId, newValue.Key, newValue.Value));
                }
            }

            foreach (var template in templates)
            {
                var isFoundInTemplates = command.CustomFields.Any(x => x.Key == template.Key);
                var isFoundInValues = existingValues.Any(x => x.Key == template.Key);
                if (!isFoundInTemplates && isFoundInValues)
                    cmdList.Add(new DeleteCustomFieldValueCommand(command.Type, command.TypeId, template.Key));
            }

            return cmdList;
        }

        private async Task AddCustomFieldValueAsync(CustomFieldValueCommand command)
        {
            var fieldValue = new CustomFieldValue
            {
                Type = command.Type,
                TypeId = command.TypeId,
                Key = command.Key,
                Value = command.Value
            };
            await BusinessRepository.AddCustomFieldValueAsync(Business.Id, fieldValue);
        }

        private async Task UpdateCustomFieldValueAsync(CustomFieldValueCommand command)
        {
            var fieldValue = new CustomFieldValue
            {
                Type = command.Type,
                TypeId = command.TypeId,
                Key = command.Key,
                Value = command.Value
            };
            await BusinessRepository.UpdateCustomFieldValueAsync(Business.Id, fieldValue);
        }

        private async Task DeleteCustomFieldValueAsync(CustomFieldValueCommand command)
        {
            var fieldValue = new CustomFieldValue
            {
                Type = command.Type,
                TypeId = command.TypeId,
                Key = command.Key,
                Value = command.Value
            };
            await BusinessRepository.DeleteCustomFieldValueAsync(Business.Id, fieldValue);
        }


        private abstract class CustomFieldValueCommand
        {
            public string Type { get; set; }
            public Guid TypeId { get; set; }
            public string Key { get; set; }
            public string Value { get; set; }

            public CustomFieldValueCommand(string type, Guid typeId, string key, string value = null)
            {
                Type = type;
                TypeId = typeId;
                Key = key;
                Value = value;
            }
        }

        private class AddCustomFieldValueCommand : CustomFieldValueCommand
        {
            public AddCustomFieldValueCommand(string type, Guid typeId, string key, string value) 
                : base(type, typeId, key, value)
            { }
        }

        private class UpdateCustomFieldValueCommand : CustomFieldValueCommand
        {
            public UpdateCustomFieldValueCommand(string type, Guid typeId, string key, string value) 
                : base(type, typeId, key, value)
            { }
        }

        private class DeleteCustomFieldValueCommand : CustomFieldValueCommand
        {
            public DeleteCustomFieldValueCommand(string type, Guid typeId, string key) 
                : base(type, typeId, key)
            { }
        }
    }
}
