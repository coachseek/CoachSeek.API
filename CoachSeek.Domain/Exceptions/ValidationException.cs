using System;
using System.Collections.Generic;
using System.Linq;

namespace CoachSeek.Domain.Exceptions
{
    public class ValidationException : Exception
    {
        public List<Error> Errors { get; private set; }


        public ValidationException()
        {
            Errors = new List<Error>();
        }

        public ValidationException(string errorMessage, string field = null)
        {
            Errors = new List<Error> { new Error(errorMessage, field) };
        }

        public ValidationException(Error error)
        {
            Errors = new List<Error> { error };
        }

        public ValidationException(IEnumerable<Error> errors)
        {
            Errors = new List<Error>(errors);
        }


        public void Add(string errorMessage, string field = null)
        {
            Errors.Add(new Error(errorMessage, field));
        }

        public bool HasErrors
        {
            get { return Errors.Any(); }
        }

        public void ThrowIfErrors()
        {
            if (HasErrors)
                throw this;
        }
    }
}