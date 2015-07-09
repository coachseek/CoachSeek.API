using System;

namespace CoachSeek.Domain.Commands
{
    public class SessionKeyCommand : KeyCommand
    {
        public SessionKeyCommand()
        { }

        public SessionKeyCommand(Guid sessionId)
        {
            Id = sessionId;
        }
    }
}
