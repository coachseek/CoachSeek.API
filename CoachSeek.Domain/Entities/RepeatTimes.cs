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

        private void Validate()
        {
            // TODO: What about a never-ending service?

            if (Count <= 0)
                throw new InvalidRepeatTimes();
        }
    }
}
