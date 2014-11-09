using System;
using System.ComponentModel.DataAnnotations;

namespace CoachSeek.WebUI.Models.Api.Setup
{
    public class ApiLocationSaveCommand : ApiSaveCommand, IApiBusinessIdable
    {
        [Required]
        public Guid? BusinessId { get; set; }
   
        [Required]
        public string Name { get; set; }
    }
}