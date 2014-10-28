using System;
using System.ComponentModel.DataAnnotations;

namespace CoachSeek.WebUI.Models.Api
{
    public class ApiServiceSaveCommand : IApiBusinessIdable, IApiIdable
    {
        [Required]
        public Guid? BusinessId { get; set; }
        public Guid? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }



        //public ApiSericeDefaults Defaults { get; set; }
        public int Duration { get; set; }
        //public ApiSericeRepeatability Repeatability { get; set; }
        //[Required]
        //ApiServiceUiCommand UiDetails { get; set; }
            

        public bool IsNew()
        {
            return !Id.HasValue;
        }
    }
}