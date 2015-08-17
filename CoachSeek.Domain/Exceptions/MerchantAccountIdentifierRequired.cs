namespace CoachSeek.Domain.Exceptions
{
    public class MerchantAccountIdentifierRequired : SingleErrorException
    {
        // The error code and message is set to be same as built by ValidateModelStateAttribute.
        public MerchantAccountIdentifierRequired()
            : base("merchantaccountidentifier-required",
                   "The MerchantAccountIdentifier field is required.")
        { }
    }
}
