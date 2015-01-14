using System.Collections.Generic;
using System.Linq;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class NotFoundResponse<TData> : Response<TData> where TData : class
    {
        public NotFoundResponse()
        {
            Data = null;
            Errors = null;
        }
    }
    public class ErrorResponse<TData> : Response<TData> where TData : class
    {
        public ErrorResponse()
        {
            Errors = new List<ErrorData>();
        }
        public ErrorResponse(ErrorData error)
        {
            Errors = new List<ErrorData>();
            Errors.Add(error);
        }
    }

    public class Response<TData> where TData : class
    {
        public TData Data { get; protected set; }
        public List<ErrorData> Errors { get; protected set; }
        public bool IsSuccessful { get { return Errors == null; } }

        protected Response()
        { }

        public Response(TData data)
        {
            Data = data;
        }

        public Response(ValidationException exception)
        {
            Errors = exception.Errors.Select(error => error.ToData()).ToList();
        }
    }
}