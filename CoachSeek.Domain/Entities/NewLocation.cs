using System;
using CoachSeek.Domain.Data;

namespace CoachSeek.Domain.Entities
{
    public class NewLocation : Location
    {
        public NewLocation(string name)
            : base(Guid.NewGuid(), name)
        { }

        public NewLocation(NewLocationData data)
            : this(data.Name)
        { }
    }
}