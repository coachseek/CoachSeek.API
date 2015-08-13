using System.Collections.Generic;
using System.Linq;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Exceptions
{
    public class ValidationException : CoachseekException
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

        public ValidationException(IEnumerable<ErrorData> errors)
        {
            Errors = new List<Error>();
            foreach(var error in errors)
                Errors.Add(new Error(error.Code, error.Message, error.Data));
        }

        public ValidationException(IEnumerable<Error> errors)
        {
            Errors = new List<Error>(errors);
        }

        public ValidationException(CoachseekException exception) 
            : this(exception.ToData())
        { }


        public void Add(string errorMessage, string field = null)
        {
            Errors.Add(new Error(errorMessage, field));
        }

        public void Add(ValidationException exception)
        {
            Errors.AddRange(exception.Errors);
        }

        public void Add(SingleErrorException exception)
        {
            var errors = exception.ToData();
            foreach (var error in errors)
                Errors.Add(new Error(error.Code, error.Message, error.Data));
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

        public override IList<ErrorData> ToData()
        {
            var data = new List<ErrorData>();
            if (!HasErrors)
                return data;
            data.AddRange(Errors.Select(error => error.ToData()));
            return data;
        }
    }
}