using System.Collections.Generic;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public abstract class Response
    {
        public BusinessData Business { get; protected set; }

        public List<Error> Errors { get; protected set; }


        protected Response()
        { }

        protected Response(Business business)
        {
            Business = business == null ? null : business.ToData();
        }

        protected Response(ValidationException exception)
        {
            Errors = exception.Errors;
        }

        public bool IsSuccessful
        {
            get { return Business != null; }
        }
    }
}