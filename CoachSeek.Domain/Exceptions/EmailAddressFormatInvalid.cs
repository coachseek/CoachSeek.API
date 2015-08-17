namespace CoachSeek.Domain.Exceptions
{
    public class EmailAddressFormatInvalid : SingleErrorException
    {
        // The error code and message is set to be same as built by ValidateModelStateAttribute.
        public EmailAddressFormatInvalid()
            : base("email-invalid",
                   "The Email field is not a valid e-mail address.")
        { }
    }
}
