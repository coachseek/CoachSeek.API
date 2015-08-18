using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    public class ServiceTiming
    {
        public int? Duration { get { return _duration.Minutes; } }

        private Duration _duration { get; set; }


        public ServiceTiming(ServiceTimingCommand timing)
        {
            _duration = new Duration(timing.Duration);
        }

        public ServiceTiming(ServiceTimingData timing)
        {
            _duration = new Duration(timing.Duration);
        }


        public ServiceTimingData ToData()
        {
            return AutoMapper.Mapper.Map<ServiceTiming, ServiceTimingData>(this);
        }
    }
}
