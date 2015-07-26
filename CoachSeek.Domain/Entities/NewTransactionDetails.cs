using System;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;

namespace CoachSeek.Domain.Entities
{
    public class NewTransactionDetails : TransactionDetails
    {
        public NewTransactionDetails(TransactionStatus status, 
                                     PaymentProvider paymentProvider,
                                     DateTime transactionDate,
                                     bool isTesting)
            : base(status,
                   paymentProvider,
                   transactionDate,
                   DateTime.UtcNow,
                   isTesting)
        { }

        public NewTransactionDetails(string status,
                                     string paymentProvider,
                                     DateTime transactionDate,
                                     bool isTesting)
            : base(status.Parse<TransactionStatus>(),
                   paymentProvider.Parse<PaymentProvider>(),
                   transactionDate,
                   DateTime.UtcNow,
                   isTesting)
        { }
    }
}
