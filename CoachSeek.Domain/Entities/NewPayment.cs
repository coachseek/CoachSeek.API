using CoachSeek.Common;

namespace CoachSeek.Domain.Entities
{
    public class NewPayment : Payment
    {
        protected override TransactionType TransactionType { get { return TransactionType.Payment; } }
        private string OriginalMessage { get; set; }

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
    }
}
