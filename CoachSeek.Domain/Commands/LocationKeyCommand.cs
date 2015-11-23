using System;

namespace CoachSeek.Domain.Commands
{
    public class LocationKeyCommand : KeyCommand
    {
        public LocationKeyCommand() 
        { }

        public LocationKeyCommand(Guid id) 
            : base(id)
        { }
    }
}
