﻿using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class SessionPricing
    {
        private readonly Price _sessionPrice;
        private readonly Price _coursePrice;

        public decimal? SessionPrice { get { return _sessionPrice.Amount; } }
        public decimal? CoursePrice { get { return _coursePrice.Amount; } }


        public SessionPricing(PricingData data, ServiceData serviceData, RepetitionData repetitionData)
        {
            //data = BackfillMissingValuesFromService(data, serviceData, repetitionData);
            //Validate(data);

            _sessionPrice = new Price(data.SessionPrice);
            _coursePrice = new Price(data.CoursePrice);
        }

        public SessionPricing(decimal? sessionPrice, decimal? coursePrice)
        {
            _sessionPrice = new Price(sessionPrice);
            _coursePrice = new Price(coursePrice);
        }


        public PricingData ToData()
        {
            return Mapper.Map<SessionPricing, PricingData>(this);
        }


        private SessionBookingData BackfillMissingValuesFromService(PricingData data, ServiceData serviceData, RepetitionData repetitionData)
        {
            //if (IsSession(serviceData, repetitionData))
            //{
                
            //}


            //if (data == null &&
            //    serviceData.Pricing != null &&
            //    serviceData.Pricing.StudentCapacity.HasValue &&
            //    serviceData.Pricing.IsOnlineBookable.HasValue)
            //{
            //    return new SessionBookingData
            //    {
            //        StudentCapacity = serviceData.Defaults.StudentCapacity.Value,
            //        IsOnlineBookable = serviceData.Defaults.IsOnlineBookable.Value
            //    };
            //}

            //if (data.StudentCapacity == null && serviceData.Defaults != null)
            //    data.StudentCapacity = serviceData.Defaults.StudentCapacity;

            //if (data.IsOnlineBookable == null && serviceData.Defaults != null)
            //    data.IsOnlineBookable = serviceData.Defaults.IsOnlineBookable;

            //return data;

            return null;
        }

        //private bool IsSession(ServiceData serviceData, RepetitionData repetitionData)
        //{
        //    if (repetitionData == null && serviceData.Repetition != null )
        //    throw new System.NotImplementedException();
        //}

        private void Validate(SessionBookingData data)
        {
            var errors = new ValidationException();

            if (data.StudentCapacity == null)
                errors.Add("The studentCapacity is not valid.", "session.booking.studentCapacity");

            if (data.IsOnlineBookable == null)
                errors.Add("The isOnlineBookable is not valid.", "session.booking.isOnlineBookable");

            errors.ThrowIfErrors();
        }
    }
}
