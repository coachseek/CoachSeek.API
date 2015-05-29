using System;
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

        public void SendCourseEmailToCustomer(CourseBooking booking, RepeatedSessionData course, CoachData coach, CustomerData customer)
        {
            var subjectAndBody = CreateCourseCustomerEmailSubjectAndBody(booking, course, coach, customer);
            var email = new Email(Sender, customer.Email, subjectAndBody.Item1, subjectAndBody.Item2);
            Emailer.Send(email);
        }

        public void SendCourseEmailToCoach(CourseBooking booking, RepeatedSessionData course, CoachData coach, CustomerData customer)
        {
            var subjectAndBody = CreateCourseCoachEmailSubjectAndBody(booking, course, coach, customer);
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

        private Tuple<string, string> CreateCourseCustomerEmailSubjectAndBody(CourseBooking booking, RepeatedSessionData course, CoachData coach, CustomerData customer)
        {
            var bodyTemplate = ReadEmbeddedTextResource(TEMPLATE_RESOURCE_COURSE_CUSTOMER);
            var substitutes = CreateCourseSubstitutes(booking, course, coach, customer);
            var emailTemplate = TemplateProcessor.ProcessTemplate(bodyTemplate, substitutes);
            return ExtractSubjectAndBody(emailTemplate);
        }

        private Tuple<string, string> CreateCourseCoachEmailSubjectAndBody(CourseBooking booking, RepeatedSessionData course, CoachData coach, CustomerData customer)
        {
            var bodyTemplate = ReadEmbeddedTextResource(TEMPLATE_RESOURCE_COURSE_COACH);
            var substitutes = CreateCourseSubstitutes(booking, course, coach, customer);
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
                {"SessionPrice", CalculateSessionPrice(session).ToString("C")}
            };

            return sessionValues;
        }


        private Dictionary<string, string> CreateCourseSubstitutes(CourseBooking booking, RepeatedSessionData course, CoachData coach, CustomerData customer)
        {
            var courseValues = new Dictionary<string, string>
            {
                {"BusinessName", BusinessName},
                {"CustomerFirstName", customer.FirstName},
                {"CustomerLastName", customer.LastName},
                {"CustomerEmail", customer.Email},
                {"CustomerPhone", customer.Phone},
                {"LocationName", course.Location.Name},
                {"CoachFirstName", coach.FirstName},
                {"CoachLastName", coach.LastName},
                {"ServiceName", course.Service.Name},
                {"StartDate", ReformatDate(course.Timing.StartDate)},
                {"StartTime", course.Timing.StartTime},
                {"Duration", FormatDuration(course.Timing.Duration)},
                {"SessionCount", course.Repetition.SessionCount.ToString()},
                {"RepeatFrequency", course.Repetition.RepeatFrequency == "d" ? "days" : "weeks"},
                {"CoursePrice", CalculateCoursePrice(course).ToString("C")}
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

        private decimal CalculateCoursePrice(RepeatedSessionData course)
        {
            if (course.Pricing.CoursePrice.HasValue)
                return course.Pricing.CoursePrice.Value;

            if (course.Pricing.SessionPrice.HasValue)
                return course.Pricing.SessionPrice.Value * course.Repetition.SessionCount;
            
            return 0;
        }
    }
}
