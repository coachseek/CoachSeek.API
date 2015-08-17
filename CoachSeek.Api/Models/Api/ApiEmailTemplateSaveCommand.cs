using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api
{
    public class ApiEmailTemplateSaveCommand
    {
        public string Type { get; set; }
        [Required]
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}