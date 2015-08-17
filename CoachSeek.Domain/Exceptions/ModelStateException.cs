namespace CoachSeek.Domain.Exceptions
{
    public abstract class ModelStateException : SingleErrorException
    {
        protected ModelStateException(string errorCode, string message, string data = null)
            : base(errorCode, message, data)
        { }
    }
}