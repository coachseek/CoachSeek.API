using System;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;

namespace CoachSeek.Domain.Entities
{
    public class TransactionDetails
    {
        public TransactionStatus Status { get; private set; }
        public PaymentProvider PaymentProvider { get; private set; }
        public DateTime TransactionDate { get; private set; }


        public TransactionDetails(TransactionStatus status, 
                                  PaymentProvider paymentProvider,
                                  DateTime transactionDate)
        {
            Status = status;
            PaymentProvider = paymentProvider;
            TransactionDate = transactionDate;
        }

        public TransactionDetails(string status,
                                  string paymentProvider,
                                  DateTime transactionDate)
        {
            Status = status.Parse<TransactionStatus>();
            PaymentProvider = paymentProvider.Parse<PaymentProvider>();
            TransactionDate = transactionDate;
        }
    }
}
