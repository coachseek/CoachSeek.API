using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public abstract class Repetition
    {
        protected const int MAXIMUM_WEEKLY_REPEAT = 26;
        protected const int MAXIMUM_DAILY_REPEAT = 30;

        public int SessionCount { get { return _sessionCount.Count; } }
        public string RepeatFrequency { get { return _frequency.Frequency; } }

        protected SessionCount _sessionCount { get; set; }
        protected RepeatFrequency _frequency { get; set; }

        public bool IsOpenEnded { get { return _sessionCount.IsOpenEnded; } }
        public bool IsSingleSession { get { return _sessionCount.Count == 1; } }
        public bool IsRepeatingSession { get { return !IsSingleSession; } }
        public bool HasRepeatFrequency { get { return RepeatFrequency != null; } }
        public bool IsRepeatedEveryDay { get { return HasRepeatFrequency && _frequency.IsRepeatedEveryDay; } }
        public bool IsRepeatedEveryWeek { get { return HasRepeatFrequency && _frequency.IsRepeatedEveryWeek; } } 


        protected void CreateAndValidateRepetition(RepetitionCommand repetitionData)
        {
            var errors = new ValidationException();

            ValidateAndCreateSessionCount(repetitionData.SessionCount, errors);
            ValidateAndCreateRepeatFrequency(repetitionData.RepeatFrequency, errors);

            errors.ThrowIfErrors();

            if (IsSingleSession && HasRepeatFrequency)
                throw new ValidationException("For a single session the repeatFrequency must not be set.", RepeatFrequencyPath);
            if (IsRepeatingSession)
            {
                if (!HasRepeatFrequency)
                    throw new ValidationException("For a repeated session the repeatFrequency must be set.", RepeatFrequencyPath);
                if (_frequency.IsRepeatedEveryDay && SessionCount > MAXIMUM_DAILY_REPEAT)
                    throw new ValidationException(
                        string.Format("The maximum number of daily sessions is {0}.", MAXIMUM_DAILY_REPEAT), SessionCountPath);
                if (_frequency.IsRepeatedEveryWeek && SessionCount > MAXIMUM_WEEKLY_REPEAT)
                    throw new ValidationException(
                        string.Format("The maximum number of weekly sessions is {0}.", MAXIMUM_WEEKLY_REPEAT), SessionCountPath);
            }
        }

        protected void CreateRepetition(RepetitionData repetitionData)
        {
            _frequency = new RepeatFrequency(repetitionData.RepeatFrequency);
            _sessionCount = new SessionCount(repetitionData.SessionCount);
        }

        public abstract RepetitionData ToData();


        protected abstract string SessionCountPath { get; }
        protected abstract string RepeatFrequencyPath { get; }


        protected void ValidateAndCreateSessionCount(int count, ValidationException errors)
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

        protected void ValidateAndCreateRepeatFrequency(string frequency, ValidationException errors)
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
