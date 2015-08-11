using System.Collections.Generic;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.Contracts.Models
{
    public class ErrorResponse : IResponse
    {
        private List<ErrorData> _errors;

        public IList<ErrorData> Errors
        {
            get { return _errors ?? (_errors = new List<ErrorData>()); }
        }

        public object Data { get { return null; } }

        public bool IsSuccessful
        {
            get { return Errors.Count == 0; }
        }


        public ErrorResponse(string message, string field = null, string code = null, string data = null)
        {
            Errors.Add(new ErrorData(field, message, code, data));
        }

        public ErrorResponse(SingleErrorException exception)
        {
            Errors.Add(exception.ToError().ToData());
        }

        public ErrorResponse(ValidationException exception)
        {
            foreach (var error in exception.Errors)
                Errors.Add(error.ToData());
        }
    }
}
