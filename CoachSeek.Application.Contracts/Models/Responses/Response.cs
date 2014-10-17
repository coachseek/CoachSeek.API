using System.Collections.Generic;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class NotFoundResponse : Response
    {
        public NotFoundResponse()
        {
            Business = null;
            Errors = null;
        }
    }

    public class Response
    {
        public BusinessData Business { get; protected set; }
        public List<Error> Errors { get; protected set; }
        public bool IsSuccessful { get { return Errors == null; } }

        protected Response()
        { }

        public Response(Business business)
        {
            Business = business == null ? null : business.ToData();
        }

        public Response(ValidationException exception)
        {
            Errors = exception.Errors;
        }
    }
}