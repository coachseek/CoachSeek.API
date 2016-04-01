using System;

namespace CoachSeek.Data.Model
{
    public class DiscountCodeData
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public int DiscountPercent { get; set; }
        public bool IsActive { get; set; }
    }
}
