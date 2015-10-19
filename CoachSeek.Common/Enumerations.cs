namespace CoachSeek.Common
{
    public enum Environment
    {
        Debug = 0,
        Testing,
        Production
    }

    public enum Role
    {
        Anonymous,
        CoachseekAdmin,
        BusinessAdmin,
        Coach
    }

    public enum TransactionType
    {
        None = 0,
        Payment
    }

    public enum PaymentProvider
    {
        None = 0,
        Test,
        PayPal
    }

    public enum TransactionStatus
    {
        None = 0,
        Pending,
        Completed,
        Denied
    }

    public enum ApiDataFormat
    {
        Json,
        Xml,
        FormUrlEncoded
    }
}
