using CoachSeek.Common;

namespace CoachSeek.Domain.Entities
{
    public abstract class Transaction
    {
        private TransactionDetails Details { get; set; }
        private Payer Payer { get; set; }
        private Merchant Merchant { get; set; }
        private GoodOrService Item { get; set; }

        protected abstract TransactionType TransactionType { get; }

        public string Id { get { return Details.Id; } }
        public string Type { get { return TransactionType.ToString(); } }
        public string Status { get { return Details.Status.ToString(); } }


        protected Transaction(TransactionDetails details, 
                           Payer payer,
                           Merchant merchant, 
                           GoodOrService item)
        {
            Details = details;
            Payer = payer;
            Merchant = merchant;
            Item = item;
        }
    }
}
