using CoachSeek.Common;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Exceptions
{
    public class CustomFieldTemplateDuplicate : SingleErrorException
    {
        public CustomFieldTemplateDuplicate(CustomFieldTemplate template)
            : base(ErrorCodes.CustomFieldTemplateDuplicate,
                   string.Format("Custom Field Template of type '{0}' and for key '{1}' already exists.", template.Type, template.Key),
                   string.Format("{0}:{1}", template.Type, template.Key))
        { }
    }
}