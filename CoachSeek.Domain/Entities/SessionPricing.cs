using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public class SessionPricing
    {
        private readonly Price _sessionPrice;
        private readonly Price _coursePrice;

        public decimal? SessionPrice { get { return _sessionPrice.Amount; } }
        public decimal? CoursePrice { get { return _coursePrice.Amount; } }


        public SessionPricing(PricingData data)
            : this(data.SessionPrice, data.CoursePrice)
        { }

        public SessionPricing(decimal? sessionPrice, decimal? coursePrice)
        {
            _sessionPrice = new Price(sessionPrice);
            _coursePrice = new Price(coursePrice);
        }


        public PricingData ToData()
        {
            return Mapper.Map<SessionPricing, PricingData>(this);
        }
    }
}
