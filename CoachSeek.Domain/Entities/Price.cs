using System;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class Price
    {
        private readonly decimal? _amount;

        public decimal? Amount { get { return _amount; } }

        public Price(decimal itemAmount, int quantity)
        {
            Validate(itemAmount, quantity);

            _amount = itemAmount * quantity;
        }

        public Price(decimal? amount)
        {
            _amount = amount;

            Validate();
        }

        private void Validate(decimal itemAmount, int quantity)
        {
            if (itemAmount < 0 || quantity < 0)
                throw new InvalidPrice();
            if (GetDecimalPlaces(itemAmount) > 2)
                throw new InvalidPrice();
        }

        private void Validate()
        {
            if (Amount < 0)
                throw new InvalidPrice();
            if (GetDecimalPlaces(Amount) > 2)
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

        private int GetDecimalPlaces(decimal? amount)
        {
            return amount.HasValue ? GetDecimalPlaces(amount.Value) : 0;
        }

        private int GetDecimalPlaces(decimal amount)
        {
            return BitConverter.GetBytes(decimal.GetBits((decimal)(double)amount)[3])[2];
        }
    }
}
