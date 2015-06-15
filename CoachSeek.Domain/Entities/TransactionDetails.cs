using System;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;

namespace CoachSeek.Domain.Entities
{
    public class TransactionDetails
    {
        public TransactionStatus Status { get; private set; }
        public PaymentProvider PaymentProvider { get; private set; }
        public bool? IsTransactionVerified { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public bool IsTesting { get; private set; }

        public bool IsVerified
        {
            get { return IsTransactionVerified.HasValue; }
        }


        public TransactionDetails(TransactionStatus status, 
                                  PaymentProvider paymentProvider,
                                  DateTime transactionDate, 
                                  bool isTesting)
        {
            Status = status;
            PaymentProvider = paymentProvider;
            TransactionDate = transactionDate;
            IsTesting = isTesting;
        }

        public TransactionDetails(string status,
                                  string paymentProvider,
                                  DateTime transactionDate,
                                  bool isTesting)
        {
            Status = status.Parse<TransactionStatus>();
            PaymentProvider = paymentProvider.Parse<PaymentProvider>();
            TransactionDate = transactionDate;
            IsTesting = isTesting;
        }


        public void Verify(bool isVerified)
        {
            IsTransactionVerified = isVerified;
        }
    }
}
