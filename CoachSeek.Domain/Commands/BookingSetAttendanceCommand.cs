using System;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Domain.Commands
{
    public class BookingSetAttendanceCommand : ICommand
    {
        public Guid BookingId { get; set; }
        public bool? HasAttended { get; set; }
    }
}