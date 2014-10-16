using System;

namespace CoachSeek.WebUI.Models.Api
{
    public interface IApiIdable
    {
        Guid? Id { get; set; }
    }
}
