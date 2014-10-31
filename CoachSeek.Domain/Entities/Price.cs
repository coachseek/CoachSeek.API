using System;
using System.Xml.Schema;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class Price
    {
        private readonly decimal? _amount;

        public decimal? Amount { get { return _amount; } }

        public Price(decimal? amount)
        {
            _amount = amount;

            Validate();
        }

        private void Validate()
        {
            if (Amount < 0)
                throw new InvalidPrice();
            if (DecimalPlaces > 2)
                throw new InvalidPrice();
        }

        private int DecimalPlaces 
        {
            get
            {
                if (Amount.HasValue)
                    return BitConverter.GetBytes(decimal.GetBits((decimal)(double)Amount)[3])[2];
                return 0;
            }
        }
    }
}
