using CoachSeek.Common;

namespace CoachSeek.Domain.Entities
{
    public class Payment : Transaction
    {
        public Payment(string id, TransactionStatus status, bool isTesting)
            : base(id, TransactionType.Payment, status, isTesting)
        { }
    }
}
