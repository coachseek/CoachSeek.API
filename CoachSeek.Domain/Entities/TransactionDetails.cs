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


        public TransactionDetails(TransactionStatus status, 
                                  PaymentProvider paymentProvider,
                                  DateTime transactionDate,
                                  DateTime processedDate)
        {
            Status = status;
            PaymentProvider = paymentProvider;
            TransactionDate = transactionDate;
            ProcessedDate = processedDate;
        }

        public TransactionDetails(string status,
                                  string paymentProvider,
                                  DateTime transactionDate,
                                  DateTime processedDate) 
            : this(status.Parse<TransactionStatus>(),
                   paymentProvider.Parse<PaymentProvider>(),
                   transactionDate,
                   processedDate)
        { }
    }
}
