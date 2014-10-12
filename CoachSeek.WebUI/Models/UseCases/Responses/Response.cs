using System.Collections.Generic;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.WebUI.Models.UseCases.Responses
{
    public abstract class Response
    {
        public Business Business { get; protected set; }

        public List<Error> Errors { get; protected set; }


        protected Response()
        { }

        protected Response(Business business)
        {
            Business = business;
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