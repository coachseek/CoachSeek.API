using System;
using System.Collections.Generic;

namespace CoachSeek.Domain.Exceptions
{
    public abstract class CoachseekException : Exception
    {
        protected CoachseekException()
        {
            Errors = new List<Error>();
        }

        protected CoachseekException(string message)
            : base(message)
        {
            Errors = new List<Error>();   
        }

        public List<Error> Errors { get; protected set; }
    }
}
