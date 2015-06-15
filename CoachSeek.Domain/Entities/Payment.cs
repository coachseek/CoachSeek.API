using CoachSeek.Common;
using CoachSeek.Data.Model;

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

        public Payment(PaymentData data, bool isTesting)
            : base(data, isTesting)
        { }
    }
}
