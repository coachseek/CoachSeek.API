using System.Collections.Generic;
using System.Linq;

namespace CoachSeek.Domain.Exceptions
{
    public class ValidationException : CoachseekException
    {
        public ValidationException()
        { }

        public ValidationException(string code, string message)
        {
            Errors.Add(new Error(code, message));
        }

        public ValidationException(Error error)
        {
            Errors.Add(error);
        }

        public ValidationException(IEnumerable<Error> errors)
        {
            Errors.AddRange(errors);
        }

        public ValidationException(CoachseekException exception) 
            : this(exception.Errors)
        { }


        public void Add(string errorMessage, string field = null)
        {
            Errors.Add(new Error(errorMessage, field));
        }

        public void Add(CoachseekException exception)
        {
            Errors.AddRange(exception.Errors);
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