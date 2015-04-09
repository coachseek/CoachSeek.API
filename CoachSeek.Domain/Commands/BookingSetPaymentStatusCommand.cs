using System;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Domain.Commands
{
    public class BookingSetPaymentStatusCommand : ICommand
    {
        public Guid BookingId { get; set; }
        public string PaymentStatus { get; set; }
    }
}