using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class SessionTiming
    {
        private Date _startDate;
        private TimeOfDay _startTime;
        private SessionDuration _duration;

        public string StartDate { get { return _startDate.ToString(); } }
        public string StartTime { get { return _startTime.ToData(); } }
        public int Duration { get { return _duration.Minutes; } }

        public PointInTime Start { get { return new PointInTime(_startDate, _startTime); } }
        public PointInTime Finish 
        {
            get
            {
                var finish = new PointInTime(Start);
                finish.AddMinutes(Duration);

                return finish;
            } 
        }


        public SessionTiming(SessionTimingCommand command)
        {
            ValidateAndCreateSessionTiming(command);
        }

        public SessionTiming(SessionTimingData data)
        {
            CreateSessionTiming(data);
        }


        public SessionTimingData ToData()
        {
            return Mapper.Map<SessionTiming, SessionTimingData>(this);
        }


        private void ValidateAndCreateSessionTiming(SessionTimingCommand command)
        {
            var errors = new ValidationException();

            ValidateAndCreateStartDate(command.StartDate, errors);
            ValidateAndCreateStartTime(command.StartTime, errors);
            ValidateAndCreateDuration(command.Duration, errors);

            errors.ThrowIfErrors();
        }

        private void CreateSessionTiming(SessionTimingData data)
        {
            _startDate = new Date(data.StartDate);
            _startTime = new TimeOfDay(data.StartTime);
            _duration = new SessionDuration(data.Duration);
        }

        private void ValidateAndCreateStartDate(string startDate, ValidationException errors)
        {
            try
            {
                _startDate = new Date(startDate);
            }
            catch (DateInvalid ex)
            {
                errors.Add(new StartDateInvalid(ex));
            }
        }

        private void ValidateAndCreateStartTime(string startTime, ValidationException errors)
        {
            try
            {
                _startTime = new TimeOfDay(startTime);
            }
            catch (TimeInvalid ex)
            {
                errors.Add(new StartTimeInvalid(ex));
            }
        }

        private void ValidateAndCreateDuration(int duration, ValidationException errors)
        {
            try
            {
                _duration = new SessionDuration(duration);
            }
            catch (DurationInvalid ex)
            {
                errors.Add(ex);
            }
        }
    }
}
