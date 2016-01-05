using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api.Setup
{
    public class ApiCustomFieldSaveCommand
    {
        [Required, StringLength(50)]
        public string Type { get; set; }
        public string Key { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }
        [Required]
        public bool? IsRequired { get; set; }


        public bool IsNew() { return string.IsNullOrEmpty(Key); }
        public bool IsExisting() { return !IsNew(); }

    }
}