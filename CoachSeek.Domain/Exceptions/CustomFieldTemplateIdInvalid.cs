using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class CustomFieldTemplateIdInvalid : SingleErrorException
    {
        public CustomFieldTemplateIdInvalid(Guid templateId)
            : base(ErrorCodes.CustomFieldTemplateIdInvalid, 
                   "This custom field template does not exist.",
                   templateId.ToString())
        { }
    }
}