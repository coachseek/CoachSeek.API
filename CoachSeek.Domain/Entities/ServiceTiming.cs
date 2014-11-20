using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class ServiceTiming
    {
        public int? Duration { get { return _duration.Minutes; } }

        private Duration _duration { get; set; }


        public ServiceTiming(ServiceTimingData timing)
        {
            try
            {
                _duration = new Duration(timing.Duration);
            }
            catch (InvalidDuration)
            {
                throw new ValidationException("The duration field is not valid.", "service.timing.duration");
            }
        }


        public ServiceTimingData ToData()
        {
            return AutoMapper.Mapper.Map<ServiceTiming, ServiceTimingData>(this);
        }
    }
}
