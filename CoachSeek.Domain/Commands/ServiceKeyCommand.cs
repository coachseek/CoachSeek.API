using System;

namespace CoachSeek.Domain.Commands
{
    public class ServiceKeyCommand : KeyCommand
    {
        public ServiceKeyCommand() 
        { }

        public ServiceKeyCommand(Guid id) 
            : base(id)
        { }
    }
}
