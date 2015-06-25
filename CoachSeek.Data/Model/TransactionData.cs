using System;

namespace CoachSeek.Data.Model
{
    public class TransactionData
    {
        public string Id { get; set; }
        public string PaymentProvider { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime TransactionDate { get; set; }
        public string PayerFirstName { get; set; }
        public string PayerLastName { get; set; }
        public string PayerEmail { get; set; }
        public Guid MerchantId { get; set; }
        public string MerchantName { get; set; }
        public string MerchantEmail { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemCurrency { get; set; }
        public decimal ItemAmount { get; set; }

        // Don't need OriginalMessage as it's write only (ie. only for logging).
    }
}
