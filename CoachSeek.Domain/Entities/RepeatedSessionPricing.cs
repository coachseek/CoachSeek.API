using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class RepeatedSessionPricing : SingleSessionPricing
    {
        private Price _coursePrice;

        public decimal? CoursePrice
        {
            get { return _coursePrice == null ? null : _coursePrice.Amount; }
        }


        public RepeatedSessionPricing(PricingCommand sessionPricing, RepeatedSessionPricingData servicePricing)
            : base(sessionPricing, servicePricing)
        {
            sessionPricing = BackfillMissingValuesFromService(sessionPricing, servicePricing);

            Validate(sessionPricing);

            ValidateAndCreatePricing(sessionPricing);
        }

        public RepeatedSessionPricing(decimal? sessionPrice, decimal? coursePrice)
            : base(sessionPrice)
        {
            CreateCoursePricing(coursePrice);
        }


        public new RepeatedSessionPricingData ToData()
        {
            return Mapper.Map<RepeatedSessionPricing, RepeatedSessionPricingData>(this);
        }


        private void ValidateAndCreatePricing(PricingCommand sessionPricing)
        {
            var errors = new ValidationException();

            ValidateAndCreateSessionPrice(sessionPricing.SessionPrice, errors);
            ValidateAndCreateCoursePrice(sessionPricing.CoursePrice, errors);

            errors.ThrowIfErrors();
        }

        private void ValidateAndCreateSessionPrice(decimal? sessionPrice, ValidationException errors)
        {
            try
            {
                _sessionPrice = new Price(sessionPrice);
            }
            catch (InvalidPrice)
            {
                errors.Add("The sessionPrice field is not valid.", "session.pricing.sessionPrice");
            }
        }

        private void ValidateAndCreateCoursePrice(decimal? coursePrice, ValidationException errors)
        {
            try
            {
                _coursePrice = new Price(coursePrice);
            }
            catch (InvalidPrice)
            {
                errors.Add("The coursePrice field is not valid.", "session.pricing.coursePrice");
            }
        }

        private void CreateCoursePricing(decimal? coursePrice)
        {
            _coursePrice = new Price(coursePrice);
        }

        protected override PricingCommand BackfillMissingValuesFromService(PricingCommand sessionPricing, SingleSessionPricingData servicePricing)
        {
            if (sessionPricing == null)
            {
                if (servicePricing == null)
                    return new PricingCommand();
                return new PricingCommand(servicePricing.SessionPrice, ((RepeatedSessionPricingData)servicePricing).CoursePrice);
            }

            if (servicePricing != null)
            {
                if (!sessionPricing.SessionPrice.HasValue)
                    sessionPricing.SessionPrice = servicePricing.SessionPrice;

                if (!sessionPricing.CoursePrice.HasValue)
                    sessionPricing.CoursePrice = ((RepeatedSessionPricingData)servicePricing).CoursePrice;
            }            

            return sessionPricing;
        }

        private void Validate(PricingCommand sessionPricing)
        {
            if (sessionPricing == null)
                throw new ValidationException("The pricing field is required.", "session.pricing");
        }
    }
}
