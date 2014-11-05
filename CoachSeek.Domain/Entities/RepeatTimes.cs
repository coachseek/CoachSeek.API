using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class RepeatTimes
    {
        private readonly int _count;

        public int Count { get { return _count; } }

        public RepeatTimes(int count)
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
                throw new InvalidRepeatTimes();
        }
    }
}
