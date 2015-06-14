using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Entities
{
    public class TransactionDetails
    {
        public string Id { get; private set; }
        public TransactionStatus Status { get; private set; }
        public PaymentProvider PaymentProvider { get; private set; }
        public bool? IsPaymentProviderVerified { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public bool IsTesting { get; private set; }

        public bool HasPaymentBeenVerified
        {
            get { return IsPaymentProviderVerified.HasValue; }
        }


        public TransactionDetails(string id, 
                                  TransactionStatus status, 
                                  PaymentProvider paymentProvider,
                                  DateTime transactionDate, 
                                  bool isTesting)
        {
            Id = id;
            Status = status;
            PaymentProvider = paymentProvider;
            TransactionDate = transactionDate;
            IsTesting = isTesting;
        }
    }
}
