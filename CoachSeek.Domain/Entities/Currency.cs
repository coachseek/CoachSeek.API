using System.Xml.Schema;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Domain.Entities
{
    public class Currency
    {
        public string Code { get; private set; }
        public string Symbol { get; private set; }

        public Currency(string code, ISupportedCurrencyRepository supportedCurrencyRepository)
        {
            var currency = supportedCurrencyRepository.GetByCode(code);
            if (currency.IsNotFound())
                throw new CurrencyNotSupported();
            Code = currency.Code;
            Symbol = currency.Symbol;
        }
    }
}
