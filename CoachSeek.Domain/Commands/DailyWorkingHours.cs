﻿namespace CoachSeek.Domain.Commands
{
    public class DailyWorkingHours
    {
        public bool IsAvailable { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }
    }
}
