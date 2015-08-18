using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class PriceInvalid : SingleErrorException
    {
        public decimal Price { get; private set; }


        public PriceInvalid(decimal price)
            : base(ErrorCodes.PriceInvalid,
                string.Format("A Price of {0} is not valid.", price),
                price.ToString())
        {
            Price = price;
        }

        protected PriceInvalid(string errorCode, string message, decimal price)
            : base(errorCode,
                   message,
                   price.ToString())
        {
            Price = price;
        }
    }
}
