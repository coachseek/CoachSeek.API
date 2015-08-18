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

        public bool IsSingleSession { get { return _sessionCount.Count == 1; } }
        public bool IsCourse { get { return !IsSingleSession; } }
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
                throw new StandaloneSessionMustHaveNoRepeatFrequency();
            if (IsCourse)
            {
                if (!HasRepeatFrequency)
                    throw new CourseMustHaveRepeatFrequency();
                if (_frequency.IsRepeatedEveryDay && SessionCount > MAXIMUM_DAILY_REPEAT)
                    throw new CourseExceedsMaximumNumberOfDailySessions(SessionCount, MAXIMUM_DAILY_REPEAT);
                if (_frequency.IsRepeatedEveryWeek && SessionCount > MAXIMUM_WEEKLY_REPEAT)
                    throw new CourseExceedsMaximumNumberOfWeeklySessions(SessionCount, MAXIMUM_WEEKLY_REPEAT);
            }
        }

        protected void CreateRepetition(RepetitionData repetitionData)
        {
            _frequency = new RepeatFrequency(repetitionData.RepeatFrequency);
            _sessionCount = new SessionCount(repetitionData.SessionCount);
        }

        public abstract RepetitionData ToData();


        protected void ValidateAndCreateSessionCount(int count, ValidationException errors)
        {
            try
            {
                _sessionCount = new SessionCount(count);
            }
            catch (CoachseekException ex)
            {
                errors.Add(ex);
            }
        }

        protected void ValidateAndCreateRepeatFrequency(string frequency, ValidationException errors)
        {
            try
            {
                _frequency = new RepeatFrequency(frequency);
            }
            catch (CoachseekException ex)
            {
                errors.Add(ex);
            }
        }
    }
}
