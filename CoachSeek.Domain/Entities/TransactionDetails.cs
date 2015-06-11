using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Entities
{
    public class TransactionDetails
    {
        public string Id { get; private set; }
        public TransactionStatus Status { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public bool IsTesting { get; private set; }


        public TransactionDetails(string id, 
                                  TransactionStatus status, 
                                  DateTime transactionDate, 
                                  bool isTesting)
        {
            Id = id;
            Status = status;
            TransactionDate = transactionDate;
            IsTesting = isTesting;
        }
    }
}
