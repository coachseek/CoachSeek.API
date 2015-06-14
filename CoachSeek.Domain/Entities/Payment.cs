using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Entities
{
    public class Payment : Transaction
    {
        protected override TransactionType TransactionType { get { return TransactionType.Payment; } }

        public Payment(TransactionDetails details, Payer payer, Merchant merchant, GoodOrService item, string originalMessage)
            : base(details, payer, merchant, item, originalMessage)
        { }
    }
}
