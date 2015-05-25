namespace CoachSeek.Data.Model
{
    public class CurrencyData
    {
        public string Code { get; set; }
        public string Symbol { get; set; }

        public CurrencyData(string code, string symbol)
        {
            Code = code;
            Symbol = symbol;
        }
    }
}
