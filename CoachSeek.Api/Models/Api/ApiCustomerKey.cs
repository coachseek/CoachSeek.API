using System;
using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api
{
    public class ApiCustomerKey
    {
        [Required]
        public Guid? Id { get; set; }

        public string Name { get; set; }
    }
}