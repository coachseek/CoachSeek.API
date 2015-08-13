using System.Collections.Generic;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Exceptions
{
    public class SingleErrorException : CoachseekException
    {
        public string ErrorCode { get; private set; }
        public new string Data { get; private set; }

        public SingleErrorException(string message, string errorCode = null, string data = null) 
            : base(message)
        {
            ErrorCode = errorCode;
            Data = data;
        }

        public override IList<ErrorData> ToData()
        {
            return new List<ErrorData>
            {
                new ErrorData(null, Message, ErrorCode, Data)
            };
        }
    }
}
