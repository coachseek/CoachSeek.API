using System;
using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api
{
    public class ApiServiceKey
    {
        [Required]
        public Guid? Id { get; set; }

        public string Name { get; set; }
    }
}