﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoachSeek.Api.Models.Api
{
    public class ApiPriceGetCommand
    {
        [Required]
        public IList<ApiSessionKey> Sessions { get; set; }
    }
}