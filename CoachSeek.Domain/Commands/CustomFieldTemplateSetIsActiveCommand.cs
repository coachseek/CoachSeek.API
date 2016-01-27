using System;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Domain.Commands
{
    public class CustomFieldTemplateSetIsActiveCommand : ICommand
    {
        public Guid TemplateId { get; set; }
        public bool IsActive { get; set; }
    }
}
