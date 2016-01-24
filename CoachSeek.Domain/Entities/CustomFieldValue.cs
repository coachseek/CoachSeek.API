using System;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    public class CustomFieldValue
    {
        public string Type { get; private set; }
        public Guid TypeId { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }

        public CustomFieldValue()
        {
        }

        public CustomFieldValue(CustomFieldValueUpdateCommand command)
        {
            //Type = command.Type;
            //TypeId = command.TypeId;
            Key = command.Key;
            Value = command.Value;
        }
    }
}
