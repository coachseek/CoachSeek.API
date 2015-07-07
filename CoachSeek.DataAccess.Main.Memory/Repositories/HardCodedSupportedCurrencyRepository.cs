using System.Collections.Generic;
using System.Linq;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.DataAccess.Main.Memory.Repositories
{
    public class HardCodedSupportedCurrencyRepository : ISupportedCurrencyRepository
    {
        private static CurrencyData NewZealandCurrency
        {
            get { return new CurrencyData("NZD", "$"); }
        }

        private static List<CurrencyData> Currencies { get; set; }

        static HardCodedSupportedCurrencyRepository()
        {
            Currencies = new List<CurrencyData>
            {
                NewZealandCurrency,
                new CurrencyData("AUD", "$"),
                new CurrencyData("USD", "$"),
                new CurrencyData("GBP", "£"),
                new CurrencyData("EUR", "€"),
                new CurrencyData("SEK", "kr"),
                new CurrencyData("ZAR", "R")
            };
        }

        public IList<CurrencyData> GetAll()
        {
            return Currencies;
        }


        public CurrencyData GetByCode(string currencyCode)
        {
            return Currencies.FirstOrDefault(x => x.Code == currencyCode.ToUpper()); // ?? NewZealandCurrency;
        }
    }
}