using CoachSeek.Common;

namespace CoachSeek.Domain.Entities
{
    public class NewPayment : Payment
    {
        protected override TransactionType TransactionType { get { return TransactionType.Payment; } }
        public bool IsTesting { get; private set; }
        public string OriginalMessage { get; private set; }


        public NewPayment(string id,
            bool isTesting,
            TransactionDetails details,
            Payer payer,
            Merchant merchant,
            GoodOrService item,
            string originalMessage)
            : base(id, details, payer, merchant, item)
        {
            IsTesting = isTesting;
            OriginalMessage = originalMessage;
        }

        public NewPayment(NewPayment newPayment, string updatedMerchantName)
            : this(newPayment.Id,
                   newPayment.IsTesting,
                   newPayment.Details,
                   newPayment.Payer,
                   new Merchant(newPayment.MerchantId, updatedMerchantName, newPayment.MerchantEmail),
                   newPayment.Item,
                   newPayment.OriginalMessage)
        { }
    }
}
