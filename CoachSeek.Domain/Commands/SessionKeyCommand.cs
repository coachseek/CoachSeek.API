using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Commands
{
    public class SessionKeyCommand : KeyCommand
    {
        public SessionKeyCommand() 
        { }

        public SessionKeyCommand(Guid id) 
            : base(id)
        { }

        public SessionKeyData ToData()
        {
            return new SessionKeyData(Id);
        }
    }
}
