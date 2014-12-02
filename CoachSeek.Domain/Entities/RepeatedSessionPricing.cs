﻿using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class RepeatedSessionPricing
    {
        private Price _sessionPrice;
        private Price _coursePrice;
        private readonly SessionRepetition _repetition;

        public decimal? SessionPrice { get { return _sessionPrice.Amount; } }
        public decimal? CoursePrice { get { return _coursePrice.Amount; } }


        public RepeatedSessionPricing(PricingData sessionPricing, PricingData servicePricing, SessionRepetition repetition)
        {
            _repetition = repetition;

            sessionPricing = BackfillMissingValuesFromService(sessionPricing, servicePricing);
            Validate(sessionPricing);

            ValidateAndCreateCoursePricing(sessionPricing);
        }

        public RepeatedSessionPricing(decimal? sessionPrice, decimal? coursePrice)
        {
            _sessionPrice = new Price(sessionPrice);
            _coursePrice = new Price(coursePrice);
        }


        public PricingData ToData()
        {
            return Mapper.Map<RepeatedSessionPricing, PricingData>(this);
        }


        private void ValidateAndCreateCoursePricing(PricingData sessionPricing)
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

        private PricingData BackfillMissingValuesFromService(PricingData sessionPricing, PricingData servicePricing)
        {
            if (sessionPricing == null)
                return servicePricing;

            if (servicePricing != null)
            {
                if (!sessionPricing.SessionPrice.HasValue)
                    sessionPricing.SessionPrice = servicePricing.SessionPrice;

                if (!sessionPricing.CoursePrice.HasValue)
                    sessionPricing.CoursePrice = servicePricing.CoursePrice;
            }

            return sessionPricing;
        }

        private void Validate(PricingData sessionPricing)
        {
            if (sessionPricing == null)
                throw new ValidationException("The pricing field is required.", "session.pricing");
        }
    }
}
