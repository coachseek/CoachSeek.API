using System;

namespace CoachSeek.Data.Model
{
    public abstract class BookingData
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public string PaymentStatus { get; set; }
        public bool? IsOnlineBooking { get; set; }
        public int DiscountPercent { get; set; }
        public CustomerKeyData Customer { get; set; }
    }
}