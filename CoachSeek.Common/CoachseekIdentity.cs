using System.Security.Principal;

namespace CoachSeek.Common
{
    public class CoachseekIdentity : GenericIdentity
    {
        public BusinessDetails Business { get; protected set; }
        public CurrencyDetails Currency { get; protected set; }

        public CoachseekIdentity(string name, string type, BusinessDetails business, CurrencyDetails currency)
            : base(name, type)
        {
            Business = business;
            Currency = currency;
        }
    }
}
