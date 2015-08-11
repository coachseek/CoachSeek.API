using System;

namespace CoachSeek.Domain.Exceptions
{
    public class SingleErrorException : Exception
    {
        private string ErrorCode { get; set; }
        private string Data { get; set; }

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
