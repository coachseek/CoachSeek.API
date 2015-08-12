using System;

namespace CoachSeek.Domain.Exceptions
{
    public class SingleErrorException : Exception
    {
        public string ErrorCode { get; private set; }
        public new string Data { get; private set; }

        public SingleErrorException(string message, string errorCode = null, string data = null) 
            : base(message)
        {
            ErrorCode = errorCode;
            Data = data;
        }

        public Error ToError()
        {
            return new Error(Message, ErrorCode, Data);
        }
    }
}
