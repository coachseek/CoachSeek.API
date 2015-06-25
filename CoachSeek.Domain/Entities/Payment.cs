using System;
using CoachSeek.Common;
using CoachSeek.Data.Model;
using NUnit.Framework.Constraints;

namespace CoachSeek.Domain.Entities
{
    public class Payment : Transaction
    {
        protected override TransactionType TransactionType { get { return TransactionType.Payment; } }

        public Payment(string id,
                       TransactionDetails details, 
                       Payer payer, 
                       Merchant merchant, 
                       GoodOrService item)
            : base(id, details, payer, merchant, item)
        { }

        public Payment(PaymentData data)
            : base(data)
        { }

        public Payment(TransactionData data)
            : base(data)
        {
            if (data.Type != "Payment")
                throw new InvalidOperationException("Invalid transaction type.");
        }
    }
}
