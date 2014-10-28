using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public class NewService : Service
    {
        public NewService(string name)
            : base(Guid.NewGuid(), name)
        { }

        public NewService(NewServiceData data)
            : this(data.Name)
        { }
    }
}