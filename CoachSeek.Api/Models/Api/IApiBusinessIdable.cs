using System;

namespace CoachSeek.Api.Models.Api
{
    public interface IApiBusinessIdable
    {
        Guid? BusinessId { get; set; }
    }
}
