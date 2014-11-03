using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class ServiceRepetition
    {
        public string RepeatFrequency { get { return _frequency.Frequency; } }
        public int RepeatTimes { get { return _times.Count; } }

        private RepeatFrequency _frequency { get; set; }
        private RepeatTimes _times { get; set; }
        
        
        public ServiceRepetition(ServiceRepetitionData repetitionData)
        {
            var errors = new ValidationException();

            CreateRepeatFrequency(repetitionData.RepeatFrequency, errors);
            CreateRepeatTimes(repetitionData.RepeatTimes, errors);

            errors.ThrowIfErrors();
        }

        public ServiceRepetitionData ToData()
        {
            return AutoMapper.Mapper.Map<ServiceRepetition, ServiceRepetitionData>(this);
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
    }
}
