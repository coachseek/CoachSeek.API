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
            var email = new Email(Sender, customer.Email, subject, body);

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
            var body = CreateCourseCustomerEmailBody(booking);
            var email = new Email(Sender, customer.Email, subject, body);

            Emailer.Send(email);
        }

        public void SendCourseEmailToCoach(CourseBooking booking, RepeatedSessionData course, CoachData coach, CustomerData customer)
        {
            const string subject = "Online Booking Email to Coach";
            var body = CreateCourseCoachEmailBody(booking);
            var email = new Email(Sender, coach.Email, subject, body);

            Emailer.Send(email);
        }


        private string CreateSessionCustomerEmailBody(SingleSessionBooking booking, SingleSessionData session, CoachData coach, CustomerData customer)
        {
            var bodyTemplate = ReadEmbeddedTextResource(TEMPLATE_RESOURCE_SESSION_CUSTOMER);
            var substitutes = CreateSessionCustomerSubstitutes(booking, session, coach, customer);
            return TemplateProcessor.ProcessTemplate(bodyTemplate, substitutes);
        }

        private string CreateSessionCoachEmailBody(SingleSessionBooking booking, SingleSessionData session, CoachData coach, CustomerData customer)
        {
            var bodyTemplate = ReadEmbeddedTextResource(TEMPLATE_RESOURCE_SESSION_COACH);
            var substitutes = CreateSessionCoachSubstitutes(booking, session, coach, customer);
            return TemplateProcessor.ProcessTemplate(bodyTemplate, substitutes);
        }

        private string CreateCourseCustomerEmailBody(CourseBooking booking)
        {
            var bodyTemplate = ReadEmbeddedTextResource(TEMPLATE_RESOURCE_COURSE_CUSTOMER);
            //var substitutes = CreatePlaceholderSubstitutes(booking);
            //return TemplateProcessor.ProcessTemplate(bodyTemplate, substitutes);
            return "";
        }

        private string CreateCourseCoachEmailBody(CourseBooking booking)
        {
            var bodyTemplate = ReadEmbeddedTextResource(TEMPLATE_RESOURCE_COURSE_COACH);
            //var substitutes = CreatePlaceholderSubstitutes(booking);
            //return TemplateProcessor.ProcessTemplate(bodyTemplate, substitutes);
            return "";
        }

        private Dictionary<string, string> CreateSessionCustomerSubstitutes(SingleSessionBooking booking, 
                                                                            SingleSessionData session, 
                                                                            CoachData coach, 
                                                                            CustomerData customer)
        {
            var values = new Dictionary<string, string>
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

            return values;
        }

        private Dictionary<string, string> CreateSessionCoachSubstitutes(SingleSessionBooking booking, SingleSessionData session, CoachData coach, CustomerData customer)
        {
            var values = new Dictionary<string, string>
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

            return values;
        }
    }
}
