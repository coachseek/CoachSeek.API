using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class SessionTiming
    {
        private readonly Date _startDate;
        private readonly PointInTime _startTime;
        private readonly SessionDuration _duration;

        public string StartDate { get { return _startDate.ToData(); } }
        public string StartTime { get { return _startTime.ToData(); } }
        public int Duration { get { return _duration.Minutes; } }


        public SessionTiming(SessionTimingData data, ServiceData serviceData)
        {
            BackfillMissingValuesFromService(data, serviceData);
            Validate(data);

            _startDate = new Date(data.StartDate);
            _startTime = new PointInTime(data.StartTime);
            _duration = new SessionDuration(data.Duration.Value);
        }

        public SessionTiming(string startDate, string startTime, int duration)
        {
            _startDate = new Date(startDate);
            _startTime = new PointInTime(startTime);
            _duration = new SessionDuration(duration);
        }


        private void BackfillMissingValuesFromService(SessionTimingData data, ServiceData serviceData)
        {
            if (data.Duration == null && serviceData.Defaults != null)
                data.Duration = serviceData.Defaults.Duration;
        }

        private void Validate(SessionTimingData data)
        {
            var errors = new ValidationException();

            if (data.Duration == null)
                errors.Add("The duration is not valid.", "session.timing.duration");

            errors.ThrowIfErrors();
        }

        public SessionTimingData ToData()
        {
            return Mapper.Map<SessionTiming, SessionTimingData>(this);
        }
    }
}
