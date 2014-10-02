using System;

namespace CoachSeek.WebUI.Models
{
    public class Identifier
    {
        public Guid Id { get; private set; }

        public Identifier()
        {
            Id = Guid.NewGuid();
        }

        public Identifier(Guid id)
        {
            Id = id;
        }
    }
}