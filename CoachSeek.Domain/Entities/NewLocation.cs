using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    public class NewLocation : Location
    {
        public NewLocation(string name)
            : base(Guid.NewGuid(), name)
        { }

        public NewLocation(LocationAddCommand command)
            : this(command.Name)
        { }

        public NewLocation(NewLocationData data)
            : this(data.Name)
        { }
    }
}