using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Commands
{
    public class CustomerKeyCommand : KeyCommand
    {
        public CustomerKeyCommand() 
        { }

        public CustomerKeyCommand(Guid id) 
            : base(id)
        { }

        public CustomerKeyData ToData()
        {
            return new CustomerKeyData(Id);
        }
    }
}
