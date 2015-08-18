using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class ServicePricing
    {
        public decimal? SessionPrice { get { return _sessionPrice.Amount; } }
        public decimal? CoursePrice { get { return _coursePrice.Amount; } }

        private Price _sessionPrice { get; set; }
        private Price _coursePrice { get; set; }


        public ServicePricing(PricingCommand pricingCommand, ServiceRepetition repetition)
        {
            ValidateHavePrices(pricingCommand);
            ValidateAndSetPrices(pricingCommand, repetition);
        }

        public ServicePricing(RepeatedSessionPricingData pricingData)
        {
            SetPrices(pricingData);
        }


        public RepeatedSessionPricingData ToData()
        {
            return AutoMapper.Mapper.Map<ServicePricing, RepeatedSessionPricingData>(this);
        }


        private void ValidateHavePrices(PricingCommand pricingCommand)
        {
            if (!pricingCommand.SessionPrice.HasValue && !pricingCommand.CoursePrice.HasValue)
                throw new ServiceIsPricedButHasNoPrices();
        }

        private void ValidateAndSetPrices(PricingCommand pricingCommand, ServiceRepetition repetition)
        {
            var errors = new ValidationException();

            // Creation of Course price depends on creation of session price.
            ValidateAndCreateSessionPrice(pricingCommand.SessionPrice, errors);
            ValidateAndCreateCoursePrice(pricingCommand, repetition, errors);

            errors.ThrowIfErrors();
        }

        private void ValidateAndCreateSessionPrice(decimal? sessionPrice, ValidationException errors)
        {
            try
            {
                _sessionPrice = new Price(sessionPrice);
            }
            catch (PriceInvalid ex)
            {
                errors.Add(new SessionPriceInvalid(ex));
            }
        }

        private void ValidateAndCreateCoursePrice(PricingCommand pricingCommand, ServiceRepetition repetition, ValidationException errors)
        {
            try
            {
                if (repetition.IsCourse)
                {
                    if (pricingCommand.CoursePrice.HasValue)
                        _coursePrice = new Price(pricingCommand.CoursePrice.Value);
                    else
                        _coursePrice = new Price(pricingCommand.SessionPrice.GetValueOrDefault(), repetition.SessionCount);
                }
                else
                {
                    if (pricingCommand.CoursePrice.HasValue)
                        errors.Add(new ServiceForStandaloneSessionMustHaveNoCoursePrice());
                    else
                        _coursePrice = new NullPrice();
                }
            }
            catch (PriceInvalid ex)
            {
                errors.Add(new CoursePriceInvalid(ex));
            }
        }

        private void SetPrices(RepeatedSessionPricingData pricingData)
        {
            _sessionPrice = new Price(pricingData.SessionPrice);
            _coursePrice = new Price(pricingData.CoursePrice);
        }
    }
}
