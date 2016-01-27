using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api.Setup
{
    public class ApiCustomFieldValueSaveCommand
    {
        [Required, StringLength(50)]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}