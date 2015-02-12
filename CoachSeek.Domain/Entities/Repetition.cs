using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public abstract class Repetition
    {
        private const int MAXIMUM_WEEKLY_REPEAT = 26;
        private const int MAXIMUM_DAILY_REPEAT = 30;

        public int SessionCount { get { return _sessionCount.Count; } }
        public string RepeatFrequency { get { return _frequency.Frequency; } }

        protected SessionCount _sessionCount { get; set; }
        protected RepeatFrequency _frequency { get; set; }

        public bool IsOpenEnded { get { return _sessionCount.IsOpenEnded; } }
        public bool IsSingleSession { get { return _sessionCount.Count == 1; } }
        public bool IsRepeatingSession { get { return !IsSingleSession; } }
        public bool HasRepeatFrequency { get { return RepeatFrequency != null; } }


        protected void CreateAndValidateRepetition(RepetitionData repetitionData)
        {
            var errors = new ValidationException();

            CreateSessionCount(repetitionData.SessionCount, errors);
            CreateRepeatFrequency(repetitionData.RepeatFrequency, errors);

            errors.ThrowIfErrors();

            if (IsSingleSession && HasRepeatFrequency)
                throw new ValidationException("For a single session the repeatFrequency must not be set.", RepeatFrequencyPath);
            if (IsRepeatingSession)
            {
                if (!HasRepeatFrequency)
                    throw new ValidationException("For a repeated session the repeatFrequency must be set.", RepeatFrequencyPath);
                if (_frequency.IsRepeatEveryDay && SessionCount > MAXIMUM_DAILY_REPEAT)
                    throw new ValidationException(
                        string.Format("The maximum number of daily sessions is {0}.", MAXIMUM_DAILY_REPEAT), SessionCountPath);
                if (_frequency.IsRepeatEveryWeek && SessionCount > MAXIMUM_WEEKLY_REPEAT)
                    throw new ValidationException(
                        string.Format("The maximum number of weekly sessions is {0}.", MAXIMUM_WEEKLY_REPEAT), SessionCountPath);
            }
        }

        public abstract RepetitionData ToData();


        protected abstract string SessionCountPath { get; }
        protected abstract string RepeatFrequencyPath { get; }


        protected void CreateSessionCount(int count, ValidationException errors)
        {
            try
            {
                _sessionCount = new SessionCount(count);
            }
            catch (InvalidSessionCount)
            {
                errors.Add("The sessionCount field is not valid.", SessionCountPath);
            }
        }

        protected void CreateRepeatFrequency(string frequency, ValidationException errors)
        {
            try
            {
                _frequency = new RepeatFrequency(frequency);
            }
            catch (InvalidRepeatFrequency)
            {
                errors.Add("The repeatFrequency field is not valid.", RepeatFrequencyPath);
            }
        }
    }
}
