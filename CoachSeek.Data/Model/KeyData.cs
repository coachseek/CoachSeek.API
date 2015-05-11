using System;

namespace CoachSeek.Data.Model
{
    public abstract class KeyData
    {
        public Guid Id { get; set; }

        public string Name { get; set; } // Mostly for debugging, human readability.
    }
}
