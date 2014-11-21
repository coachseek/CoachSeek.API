using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class SessionCount
    {
        private readonly int _count;

        public int Count { get { return _count; } }

        public SessionCount(int count)
        {
            _count = count;

            Validate();
        }

        public bool IsOpenEnded { get { return Count == -1; } }


        private void Validate()
        {
            if (IsOpenEnded)
                return;

            if (Count <= 0)
                throw new InvalidSessionCount();
        }
    }
}
