using System.Collections.Generic;
using CoachSeek.Application.Contracts.Services.Emailing;
using CoachSeek.Common.Services.Templating;
using CoachSeek.Data.Model;
using Coachseek.Integration.Contracts.Models;

namespace CoachSeek.Application.Services.Emailing
{
    public class OnlineBookingEmailer : BusinessEmailerBase, IOnlineBookingEmailer
    {
        private const string TEMPLATE_RESOURCE_COACH = "CoachSeek.Application.Services.Emailing.Templates.OnlineBookingCoachEmail.txt";
        private const string TEMPLATE_RESOURCE_CUSTOMER = "CoachSeek.Application.Services.Emailing.Templates.OnlineBookingCustomerEmail.txt";


        public void SendEmailToCoach(BookingData booking)
        {
            const string subject = "Online Booking";
            var body = CreateCoachEmailBody(booking);

            var email = new Email(Sender, "", subject, body);

            Emailer.Send(email);
        }

        public void SendEmailToCustomer(BookingData booking)
        {
            const string subject = "Online Booking";
            var body = CreateCustomerEmailBody(booking);

            var email = new Email(Sender, "", subject, body);

            Emailer.Send(email);
        }

        private string CreateCoachEmailBody(BookingData booking)
        {
            var bodyTemplate = ReadEmbeddedTextResource(TEMPLATE_RESOURCE_COACH);
            //var substitutes = CreatePlaceholderSubstitutes(booking);
            //return TemplateProcessor.ProcessTemplate(bodyTemplate, substitutes);
            return "";
        }

        private string CreateCustomerEmailBody(BookingData
             booking)
        {
            var bodyTemplate = ReadEmbeddedTextResource(TEMPLATE_RESOURCE_CUSTOMER);
            //var substitutes = CreatePlaceholderSubstitutes(booking);
            //return TemplateProcessor.ProcessTemplate(bodyTemplate, substitutes);
            return "";
        }

        private Dictionary<string, string> CreatePlaceholderSubstitutes(RegistrationData registration)
        {
            var values = new Dictionary<string, string>();

            values.Add("BusinessName", registration.Business.Name);
            values.Add("BusinessDomain", registration.Business.Domain);
            values.Add("FirstName", registration.Admin.FirstName);
            values.Add("LastName", registration.Admin.LastName);
            values.Add("UserName", registration.Admin.Username);

            return values;
        }
    }
}
