using System;

namespace CoachSeek.Domain.Commands
{
    public class CustomFieldUpdateCommand : CustomFieldAddCommand, IIdentifiable
    {
        public Guid Id { get; set; }
        public bool? IsActive { get; set; }
    }
}
