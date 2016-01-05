using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class CustomFieldTemplateTypeInvalid : SingleErrorException
    {
        public CustomFieldTemplateTypeInvalid(string type)
            : base(ErrorCodes.CustomFieldTemplateTypeInvalid, 
                   string.Format("The custom field template type '{0}' is not valid.", type),
                   type)
        { }
    }
}