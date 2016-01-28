using System;

namespace CoachSeek.Data.Model
{
    public class CustomFieldValueData
    {
        public string Type { get; set; }
        public Guid TypeId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public CustomFieldKeyValueData ToKeyValue()
        {
            return new CustomFieldKeyValueData(Key, Value);
        }
    }
}
