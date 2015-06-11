namespace CoachSeek.Domain.Entities
{
    public class Money
    {
        public string CurrencyCode { get; private set; }
        public decimal MoneyAmount { get; private set; }

        public string Currency { get { return CurrencyCode.ToUpper(); } }
        public decimal Amount { get { return MoneyAmount; } }


        public Money(string currencyCode, decimal moneyAmount)
        {
            CurrencyCode = currencyCode;
            MoneyAmount = moneyAmount;
        }
    }
}
