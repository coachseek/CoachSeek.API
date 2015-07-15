﻿using System;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Services;
using Coachseek.Integration.Contracts.Models;
using CoachSeek.Application.Contracts.Services.Emailing;
using CoachSeek.Common.Services.Templating;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using System.Collections.Generic;

namespace CoachSeek.Application.Services.Emailing
{
    public class OnlineBookingEmailer : BusinessEmailerBase, IOnlineBookingEmailer
    {
        private const string DELIMITER = "=====";
        private const string TEMPLATE_RESOURCE_SESSION_CUSTOMER = "CoachSeek.Application.Services.Emailing.Templates.OnlineBookingSessionCustomerEmail.txt";
        private const string TEMPLATE_RESOURCE_COURSE_CUSTOMER = "CoachSeek.Application.Services.Emailing.Templates.OnlineBookingCourseCustomerEmail.txt";
        private const string TEMPLATE_RESOURCE_SESSION_COACH = "CoachSeek.Application.Services.Emailing.Templates.OnlineBookingSessionCoachEmail.txt";
        private const string TEMPLATE_RESOURCE_COURSE_COACH = "CoachSeek.Application.Services.Emailing.Templates.OnlineBookingCourseCoachEmail.txt";


        public void SendSessionEmailToCustomer(SingleSessionBooking booking, SingleSessionData session, CoachData coach, CustomerData customer)
        {
            var subjectAndBody = CreateSessionCustomerEmailSubjectAndBody(booking, session, coach, customer);
            var email = new Email(Sender, customer.Email, subjectAndBody.Item1, subjectAndBody.Item2);
            Emailer.Send(email);
        }

        public void SendSessionEmailToCoach(SingleSessionBooking booking, SingleSessionData session, CoachData coach, CustomerData customer)
        {
            var subjectAndBody = CreateSessionCoachEmailSubjectAndBody(booking, session, coach, customer);
            var recipients = new List<string> { Context.BusinessContext.AdminEmail, coach.Email };
            var email = new Email(Sender, recipients, subjectAndBody.Item1, subjectAndBody.Item2);
            Emailer.Send(email);
        }

        public void SendCourseEmailToCustomer(CourseBooking booking, CoachData coach, CustomerData customer)
        {
            var subjectAndBody = CreateCourseCustomerEmailSubjectAndBody(booking, coach, customer);
            var email = new Email(Sender, customer.Email, subjectAndBody.Item1, subjectAndBody.Item2);
            Emailer.Send(email);
        }

        public void SendCourseEmailToCoach(CourseBooking booking, CoachData coach, CustomerData customer)
        {
            var subjectAndBody = CreateCourseCoachEmailSubjectAndBody(booking, coach, customer);
            var recipients = new List<string> { Context.BusinessContext.AdminEmail, coach.Email };
            var email = new Email(Sender, recipients, subjectAndBody.Item1, subjectAndBody.Item2);
            Emailer.Send(email);
        }


        private Tuple<string, string> CreateSessionCustomerEmailSubjectAndBody(SingleSessionBooking booking, SingleSessionData session, CoachData coach, CustomerData customer)
        {
            var bodyTemplate = ReadEmbeddedTextResource(TEMPLATE_RESOURCE_SESSION_CUSTOMER);
            var substitutes = CreateSessionSubstitutes(booking, session, coach, customer);
            var emailTemplate = TemplateProcessor.ProcessTemplate(bodyTemplate, substitutes);
            return ExtractSubjectAndBody(emailTemplate);
        }

        private Tuple<string, string> CreateSessionCoachEmailSubjectAndBody(SingleSessionBooking booking, SingleSessionData session, CoachData coach, CustomerData customer)
        {
            var bodyTemplate = ReadEmbeddedTextResource(TEMPLATE_RESOURCE_SESSION_COACH);
            var substitutes = CreateSessionSubstitutes(booking, session, coach, customer);
            var emailTemplate = TemplateProcessor.ProcessTemplate(bodyTemplate, substitutes);
            return ExtractSubjectAndBody(emailTemplate);
        }

        private Tuple<string, string> CreateCourseCustomerEmailSubjectAndBody(CourseBooking booking, CoachData coach, CustomerData customer)
        {
            var bodyTemplate = ReadEmbeddedTextResource(TEMPLATE_RESOURCE_COURSE_CUSTOMER);
            var substitutes = CreateCourseSubstitutes(booking, coach, customer);
            var emailTemplate = TemplateProcessor.ProcessTemplate(bodyTemplate, substitutes);
            return ExtractSubjectAndBody(emailTemplate);
        }

        private Tuple<string, string> CreateCourseCoachEmailSubjectAndBody(CourseBooking booking, CoachData coach, CustomerData customer)
        {
            var bodyTemplate = ReadEmbeddedTextResource(TEMPLATE_RESOURCE_COURSE_COACH);
            var substitutes = CreateCourseSubstitutes(booking, coach, customer);
            var emailTemplate = TemplateProcessor.ProcessTemplate(bodyTemplate, substitutes);
            return ExtractSubjectAndBody(emailTemplate);
        }

        private Tuple<string, string> ExtractSubjectAndBody(string emailTemplate)
        {
            var pos = emailTemplate.IndexOf(DELIMITER, StringComparison.InvariantCulture);
            var subject = emailTemplate.Substring(0, pos).Trim();
            var body = emailTemplate.Substring(pos + DELIMITER.Length).Trim();
            return new Tuple<string, string>(subject, body);
        }

        private Dictionary<string, string> CreateSessionSubstitutes(SingleSessionBooking booking, 
                                                                    SingleSessionData session, 
                                                                    CoachData coach, 
                                                                    CustomerData customer)
        {
            var sessionValues = new Dictionary<string, string>
            {
                {"BusinessName", BusinessName},
                {"CustomerFirstName", customer.FirstName},
                {"CustomerLastName", customer.LastName},
                {"CustomerEmail", customer.Email},
                {"CustomerPhone", customer.Phone},
                {"LocationName", session.Location.Name},
                {"CoachFirstName", coach.FirstName},
                {"CoachLastName", coach.LastName},
                {"ServiceName", session.Service.Name},
                {"Date", ReformatDate(session.Timing.StartDate)},
                {"StartTime", session.Timing.StartTime},
                {"Duration", FormatDuration(session.Timing.Duration)},
                {"CurrencySymbol", CurrencySymbol},
                {"SessionPrice", CalculateSessionPrice(session).ToString("0.00")}
            };

            return sessionValues;
        }


        private Dictionary<string, string> CreateCourseSubstitutes(CourseBooking booking, CoachData coach, CustomerData customer)
        {
            var courseValues = new Dictionary<string, string>
            {
                {"BusinessName", BusinessName},
                {"CustomerFirstName", customer.FirstName},
                {"CustomerLastName", customer.LastName},
                {"CustomerEmail", customer.Email},
                {"CustomerPhone", customer.Phone},
                {"LocationName", booking.Course.Location.Name},
                {"CoachFirstName", coach.FirstName},
                {"CoachLastName", coach.LastName},
                {"ServiceName", booking.Course.Service.Name},
                {"StartDate", ReformatDate(booking.Course.Timing.StartDate)},
                {"StartTime", booking.Course.Timing.StartTime},
                {"Duration", FormatDuration(booking.Course.Timing.Duration)},
                {"SessionCount", booking.Course.Repetition.SessionCount.ToString()},
                {"RepeatFrequency", booking.Course.Repetition.RepeatFrequency == "d" ? "days" : "weeks"},
                {"CurrencySymbol", CurrencySymbol},
                {"BookedSessionCount", CreateBookedSessionCount(booking)},
                {"BookingPrice", booking.BookingPrice.ToString("0.00")}
            };

            // TODO: Include session values

            return courseValues;
        }

        private string ReformatDate(string date)
        {
            var datetime = date.Parse<DateTime>();

            return datetime.ToLongDateString();
        }

        private string FormatDuration(int duration)
        {
            return DurationFormatter.Format(duration);
        }

        private decimal CalculateSessionPrice(SingleSessionData session)
        {
            if (session.Pricing.SessionPrice.HasValue)
                return session.Pricing.SessionPrice.Value;

            return 0;
        }

        private string CreateBookedSessionCount(CourseBooking booking)
        {
            if (booking.Course.Sessions.Count == booking.SessionBookings.Count)
                return string.Format("Onto all {0} sessions", booking.SessionBookings.Count);
            if (booking.SessionBookings.Count == 1)
                return string.Format("Onto one session");
            return string.Format("Onto {0} sessions", booking.SessionBookings.Count);
        }
    }
}
