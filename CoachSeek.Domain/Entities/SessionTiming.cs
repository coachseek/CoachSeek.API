using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public class SessionTiming
    {
        private readonly Date _startDate;
        private readonly PointInTime _startTime;
        private readonly Duration _duration;

        public string StartDate { get { return _startDate.ToData(); } }
        public string StartTime { get { return _startTime.ToData(); } }
        public int? Duration { get { return _duration.Minutes; } }


        public SessionTiming(SessionTimingData data)
            : this(data.StartDate, data.StartTime, data.Duration)
        { }

        public SessionTiming(string startDate, string startTime, int? duration)
        {
            _startDate = new Date(startDate);
            _startTime = new PointInTime(startTime);
            _duration = new Duration(duration);
        }


        public SessionTimingData ToData()
        {
            return Mapper.Map<SessionTiming, SessionTimingData>(this);
        }
    }
}
