using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class CustomFieldValueKeyInvalid : SingleErrorException
    {
        public CustomFieldValueKeyInvalid(string type, string key)
            : base(ErrorCodes.CustomFieldValueKeyInvalid,
                   string.Format("The custom field key '{0}' for type '{1}' does not exist.", key, type))
        { }
    }
}