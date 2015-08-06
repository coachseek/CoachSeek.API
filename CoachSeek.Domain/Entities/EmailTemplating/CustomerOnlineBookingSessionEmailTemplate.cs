using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities.EmailTemplating
{
    public class CustomerOnlineBookingSessionEmailTemplate : CustomerOnlineBookingSessionEmailTemplateDefault
    {
        public CustomerOnlineBookingSessionEmailTemplate(EmailTemplateUpdateCommand command)
            : base(command)
        { }

        public CustomerOnlineBookingSessionEmailTemplate(EmailTemplateData templateData)
            : base(templateData.Subject, templateData.Body)
        { }
    }
}
