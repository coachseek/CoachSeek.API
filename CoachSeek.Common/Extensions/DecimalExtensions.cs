using System;

namespace CoachSeek.Common.Extensions
{
    public static class DecimalExtensions
    {
        public static decimal ApplyDiscount(this decimal amount, int discountPercent)
        {
            return Math.Round(amount * (1 - discountPercent / 100.0m), 2);
        }
    }
}
