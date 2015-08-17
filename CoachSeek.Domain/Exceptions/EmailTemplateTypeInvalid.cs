using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class EmailTemplateTypeInvalid : SingleErrorException
    {
        public EmailTemplateTypeInvalid(string templateType)
            : base(ErrorCodes.EmailTemplateTypeInvalid,
                   string.Format("Email template type '{0}' is not valid.", templateType),
                   templateType)
        { }
    }
}
