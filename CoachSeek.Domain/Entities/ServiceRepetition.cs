using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class ServiceRepetition
    {
        public int RepeatTimes { get { return _times.Count; } }
        public string RepeatFrequency { get { return _frequency.Frequency; } }

        private RepeatTimes _times { get; set; }
        private RepeatFrequency _frequency { get; set; }

        public bool IsOpenEnded { get { return _times.IsOpenEnded; } }
        public bool IsSingleSession { get { return _times.Count == 1; } }
        public bool IsRepeatingSession { get { return !IsSingleSession; } }
        public bool HasRepeatFrequency { get { return RepeatFrequency != null; } }

        
        public ServiceRepetition(RepetitionData repetitionData)
        {
            var errors = new ValidationException();

            CreateRepeatTimes(repetitionData.RepeatTimes, errors);
            CreateRepeatFrequency(repetitionData.RepeatFrequency, errors);

            errors.ThrowIfErrors();

            if (IsSingleSession && HasRepeatFrequency)
                throw new ValidationException("For a single session the repeatFrequency must not be set.", "service.repetition.repeatFrequency");
            if (IsRepeatingSession && !HasRepeatFrequency)
                throw new ValidationException("For a repeated session the repeatFrequency must be set.", "service.repetition.repeatFrequency");
        }

        public RepetitionData ToData()
        {
            return AutoMapper.Mapper.Map<ServiceRepetition, RepetitionData>(this);
        }


        private void CreateRepeatTimes(int times, ValidationException errors)
        {
            try
            {
                _times = new RepeatTimes(times);
            }
            catch (InvalidRepeatTimes)
            {
                errors.Add("The repeatTimes is not valid.", "service.repetition.repeatTimes");
            }
        }

        private void CreateRepeatFrequency(string frequency, ValidationException errors)
        {
            try
            {
                _frequency = new RepeatFrequency(frequency);
            }
            catch (InvalidRepeatFrequency)
            {
                errors.Add("The repeatFrequency is not valid.", "service.repetition.repeatFrequency");
            }
        }
    }
}
