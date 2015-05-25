using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Domain.Entities
{
    public class Currency
    {
        private const string DEFAULT_CURRENCY = "NZD";

        public string Code { get; private set; }
        public string Symbol { get; private set; }

        public Currency(string code, ISupportedCurrencyRepository supportedCurrencyRepository)
        {
            if (string.IsNullOrEmpty(code))
                code = DEFAULT_CURRENCY;
            var currency = supportedCurrencyRepository.GetByCode(code);
            if (currency.IsNotFound())
                throw new CurrencyNotSupported();
            Code = currency.Code;
            Symbol = currency.Symbol;
        }
    }
}
