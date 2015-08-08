using System;
using CoachSeek.Common;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities.EmailTemplating;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Factories
{
    public static class EmailTemplateFactory
    {
        public static bool IsValidTemplateType(string templateType)
        {
            return (templateType == Constants.EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_SESSION ||
                    templateType == Constants.EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_COURSE);
        }

        public static EmailTemplate BuildEmailTemplate(EmailTemplateUpdateCommand command)
        {
            if (!IsValidTemplateType(command.Type))
                throw new ValidationException("Email template type is not valid.", "emailTemplate.type");

            if (command.Type == Constants.EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_SESSION)
                return new CustomerOnlineBookingSessionEmailTemplate(command);

            if (command.Type == Constants.EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_COURSE)
                return new CustomerOnlineBookingCourseEmailTemplate(command);

            throw new InvalidOperationException("Unhandled template type.");
        }

        public static EmailTemplate BuildEmailTemplate(EmailTemplateData templateData)
        {
            if (!IsValidTemplateType(templateData.Type))
                // Throw InvalidOperationException because we should never come in here.
                throw new InvalidOperationException("Email template type is not valid.");

            if (templateData.Type == Constants.EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_SESSION)
                return new CustomerOnlineBookingSessionEmailTemplate(templateData);

            if (templateData.Type == Constants.EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_COURSE)
                return new CustomerOnlineBookingCourseEmailTemplate(templateData);

            throw new InvalidOperationException("Unhandled template type.");
        }

        public static EmailTemplate BuildDefaultEmailTemplate(string templateType)
        {
            if (!IsValidTemplateType(templateType))
                // Throw InvalidOperationException because we should never come in here.
                throw new InvalidOperationException("Email template type is not valid.");

            if (templateType == Constants.EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_SESSION)
                return new CustomerOnlineBookingSessionEmailTemplateDefault();

            if (templateType == Constants.EMAIL_TEMPLATE_ONLINE_BOOKING_CUSTOMER_COURSE)
                return new CustomerOnlineBookingCourseEmailTemplateDefault();

            throw new InvalidOperationException("Unhandled template type.");
        }
    }
}
