using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class RepeatFrequency
    {
        private readonly string _frequency;

        public string Frequency { get { return _frequency; } }

        public RepeatFrequency(string frequency)
        {
            _frequency = frequency == null ? null : frequency.Trim().ToLower();

            Validate();
        }

        private void Validate()
        {
            if (_frequency == null)
                return;

            if (!(IsRepeatEveryDay || IsRepeatEveryWeek))
                throw new InvalidRepeatFrequency();
        }

        public bool IsRepeatEveryDay
        {
            get { return _frequency == "d"; }
        }

        public bool IsRepeatEveryWeek
        {
            get { return _frequency == "w"; }
        }
    }
}
