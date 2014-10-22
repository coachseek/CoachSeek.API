using System;

namespace CoachSeek.WebUI.Models.Api
{
    public interface IApiBusinessIdable
    {
        Guid? BusinessId { get; set; }
    }
}
