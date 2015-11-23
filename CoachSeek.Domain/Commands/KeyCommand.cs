using System;

namespace CoachSeek.Domain.Commands
{
    public class KeyCommand
    {
        public Guid Id { get; set; }

        public KeyCommand() { }

        public KeyCommand(Guid id)
        {
            Id = id;
        }
    }
}
