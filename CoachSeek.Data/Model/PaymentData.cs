using CoachSeek.Common;

namespace CoachSeek.Data.Model
{
    public class PaymentData : TransactionData
    {
        public PaymentData()
        {
            Type = Constants.TRANSACTION_PAYMENT;
        }
    }
}
