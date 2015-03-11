using System;

namespace CoachSeek.Domain.Commands
{
    public class CoachKeyCommand : KeyCommand
    {
        public CoachKeyCommand()
        { }

        public CoachKeyCommand(Guid id)
        {
            Id = id;
        }
    }
}
