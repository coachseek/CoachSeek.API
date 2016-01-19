using System;

namespace CoachSeek.Data.Model
{
    public class CustomFieldTemplateData
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public bool IsRequired { get; set; }
    }
}
