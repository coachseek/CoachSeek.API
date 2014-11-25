using System;

namespace CoachSeek.Domain.Commands
{
    public interface IIdentifiable
    {
        Guid Id { get; set; }
    }
}
