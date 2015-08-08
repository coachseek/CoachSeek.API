using System.Collections.Generic;
using System.Text;
using CoachSeek.Common;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities.EmailTemplating
{
    public class CustomerOnlineBookingSessionEmailTemplateDefault : EmailTemplate
    {
        public CustomerOnlineBookingSessionEmailTemplateDefault()
            : base(BuildEmailSubject(), BuildEmailBody())
        { }

        protected CustomerOnlineBookingSessionEmailTemplateDefault(EmailTemplateUpdateCommand command)
            : base(command)
        { }

        protected CustomerOnlineBookingSessionEmailTemplateDefault(string subject, string body)
            : base(subject, body)
        { }

        public override string Type
        {
            get { return Constants.EMAIL_TEMPLATE_CUSTOMER_SESSION_BOOKING; }
        }

        public override IList<string> Placeholders
        {
            get
            {
                var placeholders = new[] 
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
                    "Date",
                    "StartTime",
                    "Duration",
                    "CurrencySymbol",
                    "SessionPrice"
                };

                return WrapPlaceholdersInDelimiter(placeholders);
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
            builder.AppendLine("Date, Time & Duration: <<Date>> at <<StartTime>> for <<Duration>>");
            builder.AppendLine("Price: <<CurrencySymbol>><<SessionPrice>>");
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

            return builder.ToString();
        }
    }
}
