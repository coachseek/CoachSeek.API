﻿using AutoMapper;
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


        public RepeatedSessionPricing(PricingCommand sessionPricing, int sessionCount)
        {
            if (sessionPricing.SessionPrice == null && sessionPricing.CoursePrice == null)
                throw new ValidationException("At least a session or course price must be specified.", "session.pricing");

            ValidateAndCreatePricing(sessionPricing, sessionCount);
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


        private void ValidateAndCreatePricing(PricingCommand sessionPricing, int sessionCount)
        {
            var errors = new ValidationException();

            ValidateAndCreateSessionPrice(sessionPricing.SessionPrice, errors);
            ValidateAndCreateCoursePrice(sessionPricing.CoursePrice, sessionCount, errors);

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

        private void ValidateAndCreateCoursePrice(decimal? coursePrice, int sessionCount,  ValidationException errors)
        {
            try
            {
                if (coursePrice.HasValue)
                    _coursePrice = new Price(coursePrice);
                else
                    _coursePrice = new Price(_sessionPrice.Amount * sessionCount);
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
    }
}
