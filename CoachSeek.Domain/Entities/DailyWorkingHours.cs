using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class DailyWorkingHours
    {
        private PointInTime _start;
        private PointInTime _finish;

        public bool IsAvailable { get; private set; }
        public string StartTime { get { return _start != null ? _start.ToData() : null; } }
        public string FinishTime { get { return _finish != null ? _finish.ToData() : null; } }


        public DailyWorkingHours(DailyWorkingHoursData data)
        {
            if (data.IsAvailable)
                CreateAvailableWorkingHours(data.StartTime, data.FinishTime);
            else
                CreateUnavailableWorkingHours();
        }


        public DailyWorkingHoursData ToData()
        {
            return new DailyWorkingHoursData
            {
                IsAvailable = IsAvailable,
                StartTime = StartTime,
                FinishTime = FinishTime
            };
        }


        private void CreateAvailableWorkingHours(string startTime, string finishTime)
        {
            IsAvailable = true;

            try
            {
                _start = new PointInTime(startTime);
                _finish = new PointInTime(finishTime);
            }
            catch (Exception)
            {
                throw new InvalidDailyWorkingHours();
            }

            if (!_finish.IsAfter(_start))
                throw new InvalidDailyWorkingHours();
        }

        private void CreateUnavailableWorkingHours()
        {
            IsAvailable = false;

            _start = null;
            _finish = null;
        }
    }
}
