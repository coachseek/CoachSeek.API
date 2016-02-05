using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class CustomFieldTemplateIsActiveRequired : SingleErrorException
    {
        public CustomFieldTemplateIsActiveRequired()
            : base(ErrorCodes.CustomFieldTemplateIsActiveRequired,
                   "The custom field template isActive field is required.")
        { }
    }
}