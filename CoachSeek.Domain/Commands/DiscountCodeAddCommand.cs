namespace CoachSeek.Domain.Commands
{
    public class DiscountCodeAddCommand
    {
        public string Code { get; set; }
        public int DiscountPercent { get; set; }
        public bool? IsActive { get; set; }
    }
}
