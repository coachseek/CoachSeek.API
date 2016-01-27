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
        public async Task<IResponse> UpdateCustomerCustomFieldValuesAsync(CustomFieldValueListUpdateCommand command)
        {
            var templates = await LookupCustomFieldTemplatesForTypeAsync();
            var values = await LookupCustomFieldValuesByTypeIdAsync(command);
            ValidateUpdate(command, templates);
            var cmdList = SplitIntoAddUpdateDeleteCommands(command, templates, values);
            foreach (var cmd in cmdList)
            {
                if (cmd is AddCustomFieldValueCommand)
                    await AddCustomFieldValueAsync(cmd);
                if (cmd is UpdateCustomFieldValueCommand)
                    await UpdateCustomFieldValueAsync(cmd);
                if (cmd is DeleteCustomFieldValueCommand)
                    await DeleteCustomFieldValueAsync(cmd);
            }

            // TODO: Return list of validation errors.
            return new Response();
        }

        private async Task<IList<CustomFieldTemplateData>> LookupCustomFieldTemplatesForTypeAsync()
        {
            return await BusinessRepository.GetCustomFieldTemplatesAsync(Business.Id, Constants.CUSTOM_FIELD_TYPE_CUSTOMER);            
        }

        private async Task<IList<CustomFieldValueData>> LookupCustomFieldValuesByTypeIdAsync(CustomFieldValueListUpdateCommand command)
        {
            return await BusinessRepository.GetCustomFieldValuesAsync(Business.Id, command.Type, command.TypeId);
        }

        private void ValidateUpdate(CustomFieldValueListUpdateCommand command, IList<CustomFieldTemplateData> templates)
        {
            var errors = new ValidationException();
            ValidateValueKeys(command, templates, errors);
            ValidateRequiredValues(command, templates, errors);
            errors.ThrowIfErrors();
        }

        private void ValidateValueKeys(CustomFieldValueListUpdateCommand command, IList<CustomFieldTemplateData> templates, ValidationException errors)
        {
            foreach (var fieldValue in command.Values)
            {
                var isFound = templates.Any(template => template.Key == fieldValue.Key);
                if (!isFound)
                    errors.Add(new CustomFieldValueKeyInvalid(Constants.CUSTOM_FIELD_TYPE_CUSTOMER, fieldValue.Key));
            }
        }

        private void ValidateRequiredValues(CustomFieldValueListUpdateCommand command, IList<CustomFieldTemplateData> templates, ValidationException errors)
        {
            foreach (var template in templates)
            {
                var isFound = command.Values.Any(fieldValue => template.Key == fieldValue.Key);
                if (!isFound && template.IsRequired)
                    errors.Add(new CustomFieldValueRequired(Constants.CUSTOM_FIELD_TYPE_CUSTOMER, template.Key));
            }
        }

        private IList<CustomFieldValueCommand> SplitIntoAddUpdateDeleteCommands(CustomFieldValueListUpdateCommand command, 
                                                                                IList<CustomFieldTemplateData> templates, 
                                                                                IList<CustomFieldValueData> existingValues)
        {
            var cmdList = new List<CustomFieldValueCommand>();

            foreach (var newValue in command.Values)
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
                var isFoundInTemplates = command.Values.Any(x => x.Key == template.Key);
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
