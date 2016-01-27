using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class CustomFieldValueRequired : SingleErrorException
    {
        public CustomFieldValueRequired(string type, string key)
            : base(ErrorCodes.CustomFieldValueRequired,
                   string.Format("A value for custom field key '{0}' for type '{1}' is required.", key, type))
        { }
    }
}