using System;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;

namespace CoachSeek.Domain.Entities
{
    public class NewTransactionDetails : TransactionDetails
    {
        public NewTransactionDetails(TransactionStatus status, 
                                     PaymentProvider paymentProvider,
                                     DateTime transactionDate)
            : base(status,
                   paymentProvider,
                   transactionDate,
                   DateTime.UtcNow)
        { }

        public NewTransactionDetails(string status,
                                     string paymentProvider,
                                     DateTime transactionDate)
            : base(status.Parse<TransactionStatus>(),
                   paymentProvider.Parse<PaymentProvider>(),
                   transactionDate,
                   DateTime.UtcNow)
        { }
    }
}
