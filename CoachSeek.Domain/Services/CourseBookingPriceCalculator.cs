﻿using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Services
{
    public static class CourseBookingPriceCalculator
    {
        public static decimal CalculatePrice(CourseBookingData booking,
                                             RepeatedSessionData course,
                                             bool useProRataPricing,
                                             int discountPercent)
        {
            return CalculatePrice(booking.SessionBookings.Select(x => x.Session).AsReadOnly(),
                                  course.Sessions.AsReadOnly(),
                                  useProRataPricing,
                                  discountPercent,
                                  course.Pricing.CoursePrice);
        }

        public static decimal CalculatePrice(IReadOnlyCollection<SessionKeyCommand> bookedSessions, 
                                             RepeatedSessionData course,
                                             bool useProRataPricing,
                                             int discountPercent)
        {
            return CalculatePrice(bookedSessions.Select(x => x.ToData()).AsReadOnly(),
                                  course.Sessions.AsReadOnly(),
                                  useProRataPricing,
                                  discountPercent,
                                  course.Pricing.CoursePrice);
        }

        public static decimal CalculatePrice(IReadOnlyCollection<BookingSessionData> bookedSessions,
                                             IReadOnlyCollection<SingleSessionData> courseSessions,
                                             bool useProRataPricing,
                                             int discountPercent,
                                             decimal? coursePrice = null)
        {
            return CalculatePrice(bookedSessions.Select(x => new SessionKeyData(x.Id)).AsReadOnly(),
                                  courseSessions,
                                  useProRataPricing,
                                  discountPercent,
                                  coursePrice);
        }

        private static decimal CalculatePrice(IReadOnlyCollection<SessionKeyData> bookedSessions,
                                              IReadOnlyCollection<SingleSessionData> courseSessions,
                                              bool useProRataPricing, 
                                              int discountPercent,
                                              decimal? coursePrice = null)
        {
            if (!bookedSessions.Any())
                return 0;
            ValidateCanCalculatePrice(bookedSessions, courseSessions, coursePrice);
            if (bookedSessions.Count == courseSessions.Count)
                return CalculateWholeCoursePrice(bookedSessions, courseSessions, coursePrice, discountPercent);
            return CalculatePartialCoursePrice(bookedSessions, courseSessions, coursePrice, useProRataPricing, discountPercent);
        }

        private static void ValidateCanCalculatePrice(IReadOnlyCollection<SessionKeyData> bookedSessions,
                                                      IReadOnlyCollection<SingleSessionData> courseSessions,
                                                      decimal? coursePrice)
        {
            if (coursePrice.HasValue)
                return;
            foreach (var bookedSession in bookedSessions)
            {
                var sessionPrice = courseSessions.Single(x => x.Id == bookedSession.Id).Pricing.SessionPrice;
                if (!sessionPrice.HasValue)
                    throw new ArgumentException("Must have either session or course price.");
            }
        }

        private static decimal CalculateWholeCoursePrice(IReadOnlyCollection<SessionKeyData> bookedSessions,
                                                         IReadOnlyCollection<SingleSessionData> courseSessions,
                                                         decimal? coursePrice,
                                                         int discountPercent)
        {
            if (coursePrice.HasValue)
                return coursePrice.Value.ApplyDiscount(discountPercent);
            return SumUpSessionPricesForWholeCourse(bookedSessions, courseSessions).ApplyDiscount(discountPercent);
        }

        private static decimal CalculatePartialCoursePrice(IReadOnlyCollection<SessionKeyData> bookedSessions,
                                                           IReadOnlyCollection<SingleSessionData> courseSessions,
                                                           decimal? coursePrice,
                                                           bool useProRataPricing,
                                                           int discountPercent)
        {
            return SumUpSessionPricesForPartialCourse(bookedSessions, courseSessions, coursePrice, useProRataPricing).ApplyDiscount(discountPercent);
        }

        private static decimal SumUpSessionPricesForWholeCourse(IReadOnlyCollection<SessionKeyData> bookedSessions,
                                                                IReadOnlyCollection<SingleSessionData> courseSessions)
        {
            return bookedSessions.Sum(bookedSession => courseSessions.Single(x => x.Id == bookedSession.Id).Pricing.SessionPrice.GetValueOrDefault());
        }

        private static decimal SumUpSessionPricesForPartialCourse(IReadOnlyCollection<SessionKeyData> bookedSessions,
                                                                  IReadOnlyCollection<SingleSessionData> courseSessions,
                                                                  decimal? coursePrice,
                                                                  bool useProRataPricing)
        {
            decimal price = 0;
            foreach (var bookedSession in bookedSessions)
            {
                var sessionPrice = courseSessions.Single(x => x.Id == bookedSession.Id).Pricing.SessionPrice;
                if (!sessionPrice.HasValue)
                {
                    if (coursePrice.HasValue && !useProRataPricing)
                        return coursePrice.Value;
                    sessionPrice = ProRataCoursePrice(coursePrice.Value, courseSessions);                    
                }
                price += sessionPrice.Value;
            }
            return price;
        }

        private static decimal ProRataCoursePrice(decimal coursePrice, IReadOnlyCollection<SingleSessionData> courseSessions)
        {
            return Math.Round(coursePrice / courseSessions.Count, 2);
        }
    }
}
