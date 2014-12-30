using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api.Setup
{
    public class ApiLocationSaveCommand : ApiSaveCommand
    {
        [Required]
        public string Name { get; set; }
    }
}