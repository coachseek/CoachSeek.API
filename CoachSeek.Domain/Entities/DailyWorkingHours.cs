using System;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class DailyWorkingHours
    {
        private TimeOfDay _start;
        private TimeOfDay _finish;

        public bool IsAvailable { get; private set; }
        public string StartTime { get { return _start != null ? _start.ToData() : null; } }
        public string FinishTime { get { return _finish != null ? _finish.ToData() : null; } }


        public DailyWorkingHours(DailyWorkingHoursData data)
        {
            if (data.IsAvailable)
                CreateAvailableWorkingHours(data.StartTime, data.FinishTime);
            else
                CreateUnavailableWorkingHours(data.StartTime, data.FinishTime);
        }

        public DailyWorkingHours(DailyWorkingHoursCommand command)
        {
            if (command.IsAvailable)
                CreateAvailableWorkingHours(command.StartTime, command.FinishTime);
            else
                CreateUnavailableWorkingHours(command.StartTime, command.FinishTime);
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
                _start = new TimeOfDay(startTime);
                _finish = new TimeOfDay(finishTime);
            }
            catch (Exception)
            {
                throw new InvalidDailyWorkingHours();
            }

            if (!_finish.IsAfter(_start))
                throw new InvalidDailyWorkingHours();
        }

        private void CreateUnavailableWorkingHours(string startTime, string finishTime)
        {
            IsAvailable = false;

            try
            {
                _start = startTime != null ? new TimeOfDay(startTime) : null;
                _finish = finishTime != null ? new TimeOfDay(finishTime) : null;
            }
            catch (Exception)
            {
                throw new InvalidDailyWorkingHours();
            }

            if (_finish == null || _start == null)
                return;

            if (!_finish.IsAfter(_start))
                throw new InvalidDailyWorkingHours();
        }
    }
}
