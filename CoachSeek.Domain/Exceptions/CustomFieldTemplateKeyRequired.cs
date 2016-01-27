using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class CustomFieldTemplateKeyRequired : SingleErrorException
    {
        public CustomFieldTemplateKeyRequired()
            : base(ErrorCodes.CustomFieldTemplateKeyRequired,
                   "The custom field template key field is required.")
        { }
    }
}