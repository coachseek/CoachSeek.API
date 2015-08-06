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
            return (templateType == Constants.EMAIL_TEMPLATE_CUSTOMER_SESSION_BOOKING ||
                    templateType == Constants.EMAIL_TEMPLATE_CUSTOMER_COURSE_BOOKING);
        }

        public static EmailTemplate BuildEmailTemplate(EmailTemplateUpdateCommand command)
        {
            if (command.Type == Constants.EMAIL_TEMPLATE_CUSTOMER_SESSION_BOOKING)
                return new CustomerOnlineBookingSessionEmailTemplate(command);

            if (command.Type == Constants.EMAIL_TEMPLATE_CUSTOMER_COURSE_BOOKING)
                return new CustomerOnlineBookingCourseEmailTemplate(command);

            throw new ValidationException("Email template type is not valid.", "emailTemplate.type");
        }

        public static EmailTemplate BuildEmailTemplate(EmailTemplateData templateData)
        {
            if (templateData.Type == Constants.EMAIL_TEMPLATE_CUSTOMER_SESSION_BOOKING)
                return new CustomerOnlineBookingSessionEmailTemplate(templateData);

            if (templateData.Type == Constants.EMAIL_TEMPLATE_CUSTOMER_COURSE_BOOKING)
                return new CustomerOnlineBookingCourseEmailTemplate(templateData);

            // Throw InvalidOperationException because we should never come in here.
            throw new InvalidOperationException("Email template type is not valid.");
        }

        public static EmailTemplate BuildDefaultEmailTemplate(string templateType)
        {
            if (templateType == Constants.EMAIL_TEMPLATE_CUSTOMER_SESSION_BOOKING)
                return new CustomerOnlineBookingSessionEmailTemplateDefault();

            if (templateType == Constants.EMAIL_TEMPLATE_CUSTOMER_COURSE_BOOKING)
                return new CustomerOnlineBookingCourseEmailTemplateDefault();

            // Throw InvalidOperationException because we should never come in here.
            throw new InvalidOperationException("Email template type is not valid.");
        }
    }
}
