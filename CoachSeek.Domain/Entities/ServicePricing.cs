﻿using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class ServicePricing
    {
        public decimal? SessionPrice { get { return _sessionPrice.Amount; } }
        public decimal? CoursePrice { get { return _coursePrice.Amount; } }

        private Price _sessionPrice { get; set; }
        private Price _coursePrice { get; set; }


        public ServicePricing(PricingData pricingData)
        {
            ValidateHavePrices(pricingData);
            ValidateAndSetPrices(pricingData);
        }

        private void ValidateHavePrices(PricingData pricingData)
        {
            if (!pricingData.SessionPrice.HasValue && !pricingData.CoursePrice.HasValue)
                throw new ValidationException("This service is priced but has neither sessionPrice nor coursePrice.", "service.pricing");
        }

        private void ValidateAndSetPrices(PricingData pricingData)
        {
            var errors = new ValidationException();

            CreateSessionPrice(pricingData.SessionPrice, errors);
            CreateCoursePrice(pricingData.CoursePrice, errors);

            errors.ThrowIfErrors();
        }

        public ServicePricing(ServicePricing pricing, int sessionCount)
        {
            _sessionPrice = pricing._sessionPrice;
            _coursePrice = new Price(pricing.SessionPrice.Value, sessionCount);
        }

        public PricingData ToData()
        {
            return AutoMapper.Mapper.Map<ServicePricing, PricingData>(this);
        }

        private void CreateSessionPrice(decimal? sessionPrice, ValidationException errors)
        {
            try
            {
                _sessionPrice = new Price(sessionPrice);
            }
            catch (InvalidPrice)
            {
                errors.Add("The sessionPrice is not valid.", "service.pricing.sessionPrice");
            }
        }

        private void CreateCoursePrice(decimal? coursePrice, ValidationException errors)
        {
            try
            {
                _coursePrice = new Price(coursePrice);
            }
            catch (InvalidPrice)
            {
                errors.Add("The coursePrice is not valid.", "service.pricing.coursePrice");
            }
        }
    }
}
