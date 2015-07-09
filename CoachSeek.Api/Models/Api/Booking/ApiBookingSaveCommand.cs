using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api.Booking
{
    public class ApiBookingSaveCommand : ApiSaveCommand
    {
        [Required]
        public IList<ApiSessionKey> Sessions { get; set; }
        [Required]
        public ApiCustomerKey Customer { get; set; }
    }
}