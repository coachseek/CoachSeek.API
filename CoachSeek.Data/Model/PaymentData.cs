using CoachSeek.Common;

namespace CoachSeek.Data.Model
{
    public class PaymentData : TransactionData
    {
        public PaymentData(string id) 
            : base(id, Constants.TRANSACTION_PAYMENT, "")
        { }
    }
}
