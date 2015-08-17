using System.Collections.Generic;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.Contracts.Models
{
    public class ErrorResponse : IResponse
    {
        private List<Error> _errors;

        public IList<Error> Errors
        {
            get { return _errors ?? (_errors = new List<Error>()); }
        }

        public object Data { get { return null; } }

        public bool IsSuccessful
        {
            get { return Errors.Count == 0; }
        }


        public ErrorResponse(string code, string message, string data = null, string field = null)
        {
            Errors.Add(new Error(code, message, data));
        }

        public ErrorResponse(CoachseekException exception)
        {
            foreach (var error in exception.Errors)
                Errors.Add(error);
        }
    }
}
