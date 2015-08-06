using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities.EmailTemplating
{
    public class CustomerOnlineBookingCourseEmailTemplate : CustomerOnlineBookingCourseEmailTemplateDefault
    {
        public CustomerOnlineBookingCourseEmailTemplate(EmailTemplateUpdateCommand command)
            : base(command)
        { }

        public CustomerOnlineBookingCourseEmailTemplate(EmailTemplateData templateData)
            : base(templateData.Subject, templateData.Body)
        { }
    }
}
