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

            if (!(IsRepeatEveryDay ||
                  IsRepeatEverySecondDay ||
                  IsRepeatEveryWeek ||
                  IsRepeatEverySecondWeek ||
                  IsRepeatEveryMonth))
                throw new InvalidRepeatFrequency();
        }

        private bool IsRepeatEveryDay
        {
            get { return _frequency == "d"; }
        }
    
        private bool IsRepeatEverySecondDay
        {
            get { return _frequency == "2d"; }
        }

        private bool IsRepeatEveryWeek
        {
            get { return _frequency == "w"; }
        }
    
        private bool IsRepeatEverySecondWeek
        {
            get { return _frequency == "2w"; }
        }

        private bool IsRepeatEveryMonth
        {
            get { return _frequency == "m"; }
        }
    }
}
