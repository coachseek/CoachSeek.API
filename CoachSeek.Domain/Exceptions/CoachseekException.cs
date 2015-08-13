using System;
using System.Collections;
using System.Collections.Generic;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Exceptions
{
    public abstract class CoachseekException : Exception
    {
        protected CoachseekException()
        { }

        protected CoachseekException(string message)
            : base(message)
        { }

        public abstract IList<ErrorData> ToData();
    }
}
