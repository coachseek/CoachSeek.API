using System;

namespace CoachSeek.Api.Models.Api
{
    public interface IApiIdable
    {
        Guid? Id { get; set; }
    }
}
