using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class SessionTiming
    {
        private Date _startDate;
        private TimeOfDay _startTime;
        private SessionDuration _duration;

        public string StartDate { get { return _startDate.ToData(); } }
        public string StartTime { get { return _startTime.ToData(); } }
        public int Duration { get { return _duration.Minutes; } }


        public SessionTiming(SessionTimingData sessionTiming, ServiceTimingData serviceTiming)
        {
            BackfillMissingValuesFromService(sessionTiming, serviceTiming);
            CreateSessionTiming(sessionTiming);
        }


        public SessionTimingData ToData()
        {
            return Mapper.Map<SessionTiming, SessionTimingData>(this);
        }


        private void BackfillMissingValuesFromService(SessionTimingData sessionTiming, ServiceTimingData serviceTiming)
        {
            if (SessionIsMissingDuration(sessionTiming) && ServiceHasDuration(serviceTiming))
                sessionTiming.Duration = serviceTiming.Duration;
        }

        private bool SessionIsMissingDuration(SessionTimingData timing)
        {
            return timing.Duration == null;
        }

        private bool ServiceHasDuration(ServiceTimingData serviceTiming)
        {
            return serviceTiming != null && serviceTiming.Duration.HasValue;
        }

        private void CreateSessionTiming(SessionTimingData data)
        {
            var errors = new ValidationException();

            ValidateAndCreateStartDate(data.StartDate, errors);
            ValidateAndCreateStartTime(data.StartTime, errors);
            ValidateAndCreateDuration(data.Duration, errors);

            errors.ThrowIfErrors();
        }

        private void ValidateAndCreateStartDate(string startDate, ValidationException errors)
        {
            try
            {
                _startDate = new Date(startDate);
            }
            catch (InvalidDate)
            {
                errors.Add("The startDate field is not valid.", "session.timing.startDate");
            }
        }

        private void ValidateAndCreateStartTime(string startTime, ValidationException errors)
        {
            try
            {
                _startTime = new TimeOfDay(startTime);
            }
            catch (InvalidTimeOfDay)
            {
                errors.Add("The startTime field is not valid.", "session.timing.startTime");
            }
        }

        private void ValidateAndCreateDuration(int? duration, ValidationException errors)
        {
            if (!duration.HasValue)
            {
                errors.Add("The duration field is required.", "session.timing.duration");
                return;
            }

            try
            {
                _duration = new SessionDuration(duration.Value);
            }
            catch (InvalidDuration)
            {
                errors.Add("The duration field is not valid.", "session.timing.duration");
            }
        }
    }
}
