using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class ServiceStudentCapacity
    {
        private readonly int? _maximum;

        public int? Maximum { get { return _maximum; } }

        public ServiceStudentCapacity(int? maximum)
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
