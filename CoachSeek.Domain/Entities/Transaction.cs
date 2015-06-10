using CoachSeek.Common;

namespace CoachSeek.Domain.Entities
{
    public class Transaction
    {
        public string Id { get; private set; }
        public TransactionType Type { get; private set; }
        public string TypeString { get { return Type.ToString(); } }
        public TransactionStatus Status { get; private set; }
        public string StatusString { get { return Status.ToString(); } }
        public bool IsTesting { get; private set; }


        public Transaction(string id, TransactionType type, TransactionStatus status, bool isTesting)
        {
            Id = id;
            Type = type;
            Status = status;
            IsTesting = isTesting;
        }
    }
}
