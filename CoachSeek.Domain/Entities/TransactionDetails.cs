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
        public DateTime ProcessedDate { get; private set; }
        public bool IsTesting { get; private set; }


        public TransactionDetails(TransactionStatus status, 
                                  PaymentProvider paymentProvider,
                                  DateTime transactionDate,
                                  DateTime processedDate,
                                  bool isTesting)
        {
            Status = status;
            PaymentProvider = paymentProvider;
            TransactionDate = transactionDate;
            ProcessedDate = processedDate;
            IsTesting = isTesting;
        }

        public TransactionDetails(string status,
                                  string paymentProvider,
                                  DateTime transactionDate,
                                  DateTime processedDate,
                                  bool isTesting) 
            : this(status.Parse<TransactionStatus>(),
                   paymentProvider.Parse<PaymentProvider>(),
                   transactionDate,
                   processedDate,
                   isTesting)
        { }
    }
}
