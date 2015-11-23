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
        public string StartTime { get { return _start != null ? _start.ToString() : null; } }
        public string FinishTime { get { return _finish != null ? _finish.ToString() : null; } }


        public DailyWorkingHours(DailyWorkingHoursCommand command, string dayOfWeek)
        {
            if (command.IsAvailable)
                CreateAvailableWorkingHours(command.StartTime, command.FinishTime, dayOfWeek);
            else
                CreateUnavailableWorkingHours(command.StartTime, command.FinishTime, dayOfWeek);
        }

        public DailyWorkingHours(DailyWorkingHoursData data, string dayOfWeek)
        {
            if (data.IsAvailable)
                CreateAvailableWorkingHours(data.StartTime, data.FinishTime, dayOfWeek);
            else
                CreateUnavailableWorkingHours(data.StartTime, data.FinishTime, dayOfWeek);
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


        private void CreateAvailableWorkingHours(string startTime, string finishTime, string dayOfWeek)
        {
            IsAvailable = true;

            try
            {
                _start = new TimeOfDay(startTime);
                _finish = new TimeOfDay(finishTime);
            }
            catch (Exception)
            {
                throw new DailyWorkingHoursInvalid(dayOfWeek);
            }

            if (!_finish.IsAfter(_start))
                throw new DailyWorkingHoursInvalid(dayOfWeek);
        }

        private void CreateUnavailableWorkingHours(string startTime, string finishTime, string dayOfWeek)
        {
            IsAvailable = false;

            try
            {
                _start = startTime != null ? new TimeOfDay(startTime) : null;
                _finish = finishTime != null ? new TimeOfDay(finishTime) : null;
            }
            catch (Exception)
            {
                throw new DailyWorkingHoursInvalid(dayOfWeek);
            }

            if (_finish == null || _start == null)
                return;

            if (!_finish.IsAfter(_start))
                throw new DailyWorkingHoursInvalid(dayOfWeek);
        }
    }
}
