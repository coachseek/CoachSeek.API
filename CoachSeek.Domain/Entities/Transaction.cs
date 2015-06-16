﻿using System;
using CoachSeek.Common;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public abstract class Transaction
    {
        private TransactionDetails Details { get; set; }
        private Payer Payer { get; set; }
        private Merchant Merchant { get; set; }
        private GoodOrService Item { get; set; }

        protected abstract TransactionType TransactionType { get; }


        public string Id { get; private set; }
        public string PaymentProvider { get { return Details.PaymentProvider.ToString(); } }
        public bool IsVerified { get { return Details.IsVerified; } }
        public string Type { get { return TransactionType.ToString(); } }
        public string Status { get { return Details.Status.ToString(); } }
        public DateTime TransactionDate { get { return Details.TransactionDate; } }
        public string PayerFirstName { get { return Payer.FirstName; } }
        public string PayerLastName { get { return Payer.LastName; } }
        public string PayerEmail { get { return Payer.Email; } }
        public string MerchantId { get { return Merchant.Id; } }
        public string MerchantName { get { return Merchant.Name; } }
        public string MerchantEmail { get { return Merchant.Email; } }
        public string ItemId { get { return Item.Id; } }
        public string ItemName { get { return Item.Name; } }
        public string ItemCurrency { get { return Item.Money.Currency; } }
        public decimal ItemAmount { get { return Item.Money.Amount; } }


        protected Transaction(string id,
                              TransactionDetails details, 
                              Payer payer,
                              Merchant merchant, 
                              GoodOrService item)
        {
            Id = id;
            Details = details;
            Payer = payer;
            Merchant = merchant;
            Item = item;
        }

        protected Transaction(TransactionData data, bool isTesting)
        {
            Id = data.Id;
            Details = new TransactionDetails(data.Status, data.PaymentProvider, data.TransactionDate, isTesting);
            Payer = new Payer(data.PayerFirstName, data.PayerLastName, data.PayerEmail);
            Merchant = new Merchant(data.MerchantId, data.MerchantName, data.MerchantEmail);
            Item = new GoodOrService(data.ItemId, data.ItemName, new Money(data.ItemCurrency, data.ItemAmount));
        }


        public void Verify(bool isVerified)
        {
            Details.Verify(isVerified);
        }
    }
}