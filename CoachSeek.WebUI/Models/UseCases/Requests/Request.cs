using System;

namespace CoachSeek.WebUI.Models.UseCases.Requests
{
    public abstract class Request
    {
        public Guid BusinessId { get; set; }
    }
}