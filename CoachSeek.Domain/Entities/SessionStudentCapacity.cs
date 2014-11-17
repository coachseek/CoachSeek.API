using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class SessionStudentCapacity
    {
        private readonly int _maximum;

        public int Maximum { get { return _maximum; } }

        // The difference between SessionStudentCapacity and the near-equivalent ServiceStudentCapacity 
        // is that the SessionStudentCapacity must have value and is therefore non-nullable.

        public SessionStudentCapacity(int maximum)
        {
            _maximum = maximum;

            Validate();
        }

        private void Validate()
        {
            if (Maximum < 0 || Maximum > 100)
                throw new InvalidStudentCapacity();
        }
    }
}
