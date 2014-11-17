using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public abstract class Repetition
    {
        public int RepeatTimes { get { return _times.Count; } }
        public string RepeatFrequency { get { return _frequency.Frequency; } }

        protected RepeatTimes _times { get; set; }
        protected RepeatFrequency _frequency { get; set; }

        public bool IsOpenEnded { get { return _times.IsOpenEnded; } }
        public bool IsSingleSession { get { return _times.Count == 1; } }
        public bool IsRepeatingSession { get { return !IsSingleSession; } }
        public bool HasRepeatFrequency { get { return RepeatFrequency != null; } }


        protected Repetition()
        { }

        protected Repetition(RepetitionData repetitionData)
        {
            var errors = new ValidationException();

            CreateRepeatTimes(repetitionData.RepeatTimes, errors);
            CreateRepeatFrequency(repetitionData.RepeatFrequency, errors);

            errors.ThrowIfErrors();

            if (IsSingleSession && HasRepeatFrequency)
                throw new ValidationException("For a single session the repeatFrequency must not be set.", RepeatFrequencyPath);
            if (IsRepeatingSession && !HasRepeatFrequency)
                throw new ValidationException("For a repeated session the repeatFrequency must be set.", RepeatFrequencyPath);
        }

        public abstract RepetitionData ToData();


        protected abstract string RepeatTimesPath { get; }
        protected abstract string RepeatFrequencyPath { get; }


        protected void CreateRepeatTimes(int times, ValidationException errors)
        {
            try
            {
                _times = new RepeatTimes(times);
            }
            catch (InvalidRepeatTimes)
            {
                errors.Add("The repeatTimes is not valid.", RepeatTimesPath);
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
                errors.Add("The repeatFrequency is not valid.", RepeatFrequencyPath);
            }
        }
    }
}
