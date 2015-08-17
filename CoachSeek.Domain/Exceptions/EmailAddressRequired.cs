namespace CoachSeek.Domain.Exceptions
{
    public class EmailAddressRequired : SingleErrorException
    {
        // The error code and message is set to be same as built by ValidateModelStateAttribute.
        public EmailAddressRequired()
            : base("email-required",
                   "The Email field is required.")
        { }
    }
}
