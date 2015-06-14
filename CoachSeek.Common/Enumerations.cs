namespace CoachSeek.Common
{
    public enum TransactionType
    {
        None = 0,
        Payment
    }

    public enum PaymentProvider
    {
        None = 0,
        PayPal
    }

    public enum TransactionStatus
    {
        None = 0,
        Pending,
        Completed,
        Denied
    }
}
