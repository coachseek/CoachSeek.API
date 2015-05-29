namespace CoachSeek.Common
{
    public class CurrencyDetails
    {
        public string Code { get; private set; }
        public string Symbol { get; private set; }


        public CurrencyDetails(string code, string symbol)
        {
            Code = code;
            Symbol = symbol;
        }
    }
}
