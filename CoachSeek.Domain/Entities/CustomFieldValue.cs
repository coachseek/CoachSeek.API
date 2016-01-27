using System;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    public class CustomFieldValue
    {
        public string Type { get; set; }
        public Guid TypeId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        //public CustomFieldValue()
        //{
        //}

        //public CustomFieldValue(CustomFieldValueUpdateCommand command)
        //{
        //    //Type = command.Type;
        //    //TypeId = command.TypeId;
        //    Key = command.Key;
        //    Value = command.Value;
        //}
    }
}
