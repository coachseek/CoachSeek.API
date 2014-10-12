using System;

namespace CoachSeek.Domain.Commands
{
    public interface IBusinessIdable
    {
        Guid BusinessId { get; set; }
    }
}
