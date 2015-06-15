﻿using System;

namespace CoachSeek.DataAccess.Main.Memory.Models
{
    public class DbTransaction
    {
        public string Id { get; set; }
        public string PaymentProvider { get; set; }
        public bool? IsVerified { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime TransactionDate { get; set; }
        public string PayerFirstName { get; set; }
        public string PayerLastName { get; set; }
        public string PayerEmail { get; set; }
        public string MerchantId { get; set; }
        public string MerchantName { get; set; }
        public string MerchantEmail { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemCurrency { get; set; }
        public decimal ItemAmount { get; set; }
        public string OriginalMessage { get; set; }
    }
}
