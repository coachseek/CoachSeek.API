using System.Collections.Generic;
using System.Linq;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.Contracts.Models
{
    public class NotFoundResponse : ErrorResponse
    {
        public NotFoundResponse()
        {
            Errors = null;
        }
    }

    public class ErrorResponse : Response
    {
        public ErrorResponse()
        {
            Errors = new List<ErrorData>();
        }

        public ErrorResponse(ErrorData error)
        {
            Errors = new List<ErrorData> { error };
        }

        public ErrorResponse(string message, string field = null, string data = null)
        {
            Errors = new List<ErrorData> { new ErrorData(field, message, data) };
        }

        public ErrorResponse(ValidationException exception)
        {
            Errors = exception.Errors.Select(error => error.ToData()).ToList();
        }
    }

    public class Response
    {
        public object Data { get; protected set; }
        public List<ErrorData> Errors { get; protected set; }


        protected Response()
        { }

        public Response(object data = null)
        {
            Data = data;
        }


        public bool IsSuccessful
        {
            get { return (Errors == null || Errors.Count == 0); }
        }
    }
}