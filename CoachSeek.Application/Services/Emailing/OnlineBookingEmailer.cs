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
        private const string TEMPLATE_RESOURCE_SESSION_CUSTOMER = "CoachSeek.Application.Services.Emailing.Templates.OnlineBookingSessionCustomerEmail.txt";
        private const string TEMPLATE_RESOURCE_COURSE_CUSTOMER = "CoachSeek.Application.Services.Emailing.Templates.OnlineBookingCourseCustomerEmail.txt";
        private const string TEMPLATE_RESOURCE_SESSION_COACH = "CoachSeek.Application.Services.Emailing.Templates.OnlineBookingSessionCoachEmail.txt";
        private const string TEMPLATE_RESOURCE_COURSE_COACH = "CoachSeek.Application.Services.Emailing.Templates.OnlineBookingCourseCoachEmail.txt";


        public void SendSessionEmailToCustomer(SingleSessionBooking booking, SingleSessionData session, CoachData coach, CustomerData customer)
        {
            const string subject = "Online Booking Email to Customer";
            var body = CreateSessionCustomerEmailBody(booking, session, coach, customer);
            var email = new Email(Sender, customer.Email, subject, body, customer.IsEmailUnsubscribed);

            Emailer.Send(email);
        }

        public void SendSessionEmailToCoach(SingleSessionBooking booking, SingleSessionData session, CoachData coach, CustomerData customer)
        {
            const string subject = "Online Booking Email to Coach";
            var body = CreateSessionCoachEmailBody(booking, session, coach, customer);
            var email = new Email(Sender, coach.Email, subject, body);

            Emailer.Send(email);
        }

        public void SendCourseEmailToCustomer(CourseBooking booking, RepeatedSessionData course, CoachData coach, CustomerData customer)
        {
            const string subject = "Online Booking Email to Customer";
            var body = CreateCourseCustomerEmailBody(booking, course, coach, customer);
            var email = new Email(Sender, customer.Email, subject, body, customer.IsEmailUnsubscribed);

            Emailer.Send(email);
        }

        public void SendCourseEmailToCoach(CourseBooking booking, RepeatedSessionData course, CoachData coach, CustomerData customer)
        {
            const string subject = "Online Booking Email to Coach";
            var body = CreateCourseCoachEmailBody(booking, course, coach, customer);
            var email = new Email(Sender, coach.Email, subject, body);

            Emailer.Send(email);
        }


        private string CreateSessionCustomerEmailBody(SingleSessionBooking booking, SingleSessionData session, CoachData coach, CustomerData customer)
        {
            var bodyTemplate = ReadEmbeddedTextResource(TEMPLATE_RESOURCE_SESSION_CUSTOMER);
            var substitutes = CreateSessionSubstitutes(booking, session, coach, customer);
            return TemplateProcessor.ProcessTemplate(bodyTemplate, substitutes);
        }

        private string CreateSessionCoachEmailBody(SingleSessionBooking booking, SingleSessionData session, CoachData coach, CustomerData customer)
        {
            var bodyTemplate = ReadEmbeddedTextResource(TEMPLATE_RESOURCE_SESSION_COACH);
            var substitutes = CreateSessionSubstitutes(booking, session, coach, customer);
            return TemplateProcessor.ProcessTemplate(bodyTemplate, substitutes);
        }

        private string CreateCourseCustomerEmailBody(CourseBooking booking, RepeatedSessionData course, CoachData coach, CustomerData customer)
        {
            var bodyTemplate = ReadEmbeddedTextResource(TEMPLATE_RESOURCE_COURSE_CUSTOMER);
            var substitutes = CreateCourseSubstitutes(booking, course, coach, customer);
            return TemplateProcessor.ProcessTemplate(bodyTemplate, substitutes);
        }

        private string CreateCourseCoachEmailBody(CourseBooking booking, RepeatedSessionData course, CoachData coach, CustomerData customer)
        {
            var bodyTemplate = ReadEmbeddedTextResource(TEMPLATE_RESOURCE_COURSE_COACH);
            var substitutes = CreateCourseSubstitutes(booking, course, coach, customer);
            return TemplateProcessor.ProcessTemplate(bodyTemplate, substitutes);
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
                {"Date", session.Timing.StartDate},
                {"StartTime", session.Timing.StartTime},
                {"Duration", session.Timing.Duration.ToString()},
                {"SessionPrice", session.Pricing.SessionPrice.Value.ToString("C")}
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
                {"StartDate", course.Timing.StartDate},
                {"StartTime", course.Timing.StartTime},
                {"Duration", course.Timing.Duration.ToString()},
                {"SessionCount", course.Repetition.SessionCount.ToString()},
                {"RepeatFrequency", course.Repetition.RepeatFrequency == "d" ? "days" : "weeks"},
                {"CoursePrice", CalculateCoursePrice(course).ToString("C")}
            };

            // TODO: Include session values

            return courseValues;
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
