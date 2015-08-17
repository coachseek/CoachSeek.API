namespace CoachSeek.Domain.Exceptions
{
    public class SingleErrorException : CoachseekException
    {
        public string ErrorCode { get; private set; }
        public new string Data { get; private set; }

        public SingleErrorException(string errorCode, string message, string data = null) 
            : base(message)
        {
            ErrorCode = errorCode;
            Data = data;

            Errors.Add(new Error(errorCode, message, data));
        }
    }
}
