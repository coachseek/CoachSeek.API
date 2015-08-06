using System.Collections.Generic;
using System.Text;
using CoachSeek.Common;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities.EmailTemplating
{
    public class CustomerOnlineBookingCourseEmailTemplateDefault : EmailTemplate
    {
        public CustomerOnlineBookingCourseEmailTemplateDefault()
            : base(BuildEmailSubject(), BuildEmailBody())
        { }

        protected CustomerOnlineBookingCourseEmailTemplateDefault(EmailTemplateUpdateCommand command)
            : base(command)
        { }

        protected CustomerOnlineBookingCourseEmailTemplateDefault(string subject, string body)
            : base(subject, body)
        { }

        public override string Type
        {
            get { return Constants.EMAIL_TEMPLATE_CUSTOMER_COURSE_BOOKING; }
        }

        public override IList<string> Placeholders
        {
            get
            {
                return new[] 
                { 
                    "BusinessName", 
                    "CustomerFirstName",
                    "CustomerLastName",
                    "CustomerEmail",
                    "CustomerPhone",
                    "LocationName",
                    "CoachFirstName",
                    "CoachLastName",
                    "ServiceName",
                    "StartDate",
                    "StartTime",
                    "Duration",
                    "SessionCount",
                    "RepeatFrequency",
                    "CurrencySymbol",
                    "BookedSessionCount",
                    "BookedSessions",
                    "BookingPrice"
                };
            }
        }


        private static string BuildEmailSubject()
        {
            return "Confirmation of Booking with <<BusinessName>>";
        }

        private static string BuildEmailBody()
        {
            var builder = new StringBuilder();

            builder.AppendLine("Hi <<CustomerFirstName>>!");
            builder.AppendLine("Thank you for booking with <<BusinessName>>!");
            builder.AppendLine("");
            builder.AppendLine("Your booking details are below:");
            builder.AppendLine("Location: <<LocationName>>");
            builder.AppendLine("Coach: <<CoachFirstName>> <<CoachLastName>>");
            builder.AppendLine("Service: <<ServiceName>>");
            builder.AppendLine("Timing: Starting on <<StartDate>> at <<StartTime>> for <<Duration>>, occurring for a total of <<SessionCount>> <<RepeatFrequency>>");
            builder.AppendLine("<<BookedSessionCount>>:");
            builder.AppendLine("<<BookedSessions>>");
            builder.AppendLine("Price: <<CurrencySymbol>><<BookingPrice>>");
            builder.AppendLine("");
            builder.AppendLine("Your details:");
            builder.AppendLine("Name: <<CustomerFirstName>> <<CustomerLastName>>");
            builder.AppendLine("Email: <<CustomerEmail>>");
            builder.AppendLine("Phone: <<CustomerPhone>>");
            builder.AppendLine("");
            builder.AppendLine("Thank you for your business! We're looking forward to seeing you there!");
            builder.AppendLine("");
            builder.AppendLine("Warm regards,");
            builder.AppendLine("");
            builder.AppendLine("<<CoachFirstName>> <<CoachLastName>>");
            builder.AppendLine("<<BusinessName>>");
            builder.AppendLine("");
            builder.Append("Online booking powered by Coachseek (http://www.coachseek.com)");

            return builder.ToString();
        }
    }
}
