using System.Collections.Generic;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Repositories
{
    public interface ISupportedCurrencyRepository
    {
        IList<CurrencyData> GetAll();
        CurrencyData GetByCode(string currencyCode);
    }
}
