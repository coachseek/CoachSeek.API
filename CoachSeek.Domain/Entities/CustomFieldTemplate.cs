using System;
using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class CustomFieldTemplate
    {
        const string TYPE_CUSTOMER = "customer";

        public string Type { get; private set; }
        public string Key { get; private set; }
        public string Name { get; private set; }
        public bool IsRequired { get; private set; }


        public CustomFieldTemplate(CustomFieldAddCommand command)
        {
            Type = command.Type;
            Name = command.Name;
            Key = CreateKeyFromName(command.Name);
            IsRequired = command.IsRequired;

            Validate();
        }

        public CustomFieldTemplateData ToData()
        {
            return Mapper.Map<CustomFieldTemplate, CustomFieldTemplateData>(this);
        }


        private void Validate()
        {
            if (Type != TYPE_CUSTOMER)
                throw new CustomFieldTemplateTypeInvalid(Type);
        }

        private string CreateKeyFromName(string name)
        {
            return RemoveNonAlphaNumericCharacters(name).ToLowerInvariant();
        }

        private static string RemoveNonAlphaNumericCharacters(string name)
        {
            var array = name.ToCharArray();
            array = Array.FindAll(array, (c => (char.IsLetterOrDigit(c))));
            name = new string(array);
            return name;
        }
    }
}
