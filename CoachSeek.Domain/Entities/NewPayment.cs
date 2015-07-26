using CoachSeek.Common;

namespace CoachSeek.Domain.Entities
{
    public class NewPayment : Payment
    {
        protected override TransactionType TransactionType { get { return TransactionType.Payment; } }
        public string OriginalMessage { get; private set; }


        public NewPayment(string id,
            TransactionDetails details,
            Payer payer,
            Merchant merchant,
            GoodOrService item,
            string originalMessage)
            : base(id, details, payer, merchant, item)
        {
            OriginalMessage = originalMessage;
        }

        public NewPayment(NewPayment newPayment, string updatedMerchantName)
            : this(newPayment.Id,
                   newPayment.Details,
                   newPayment.Payer,
                   new Merchant(newPayment.MerchantId, updatedMerchantName, newPayment.MerchantEmail),
                   newPayment.Item,
                   newPayment.OriginalMessage)
        { }
    }
}
