using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    class BookUntil    {
        public int Minutes { get; private set; }

        public BookUntil(int minutes)
        {
            Minutes = minutes;
        }
    }
}
