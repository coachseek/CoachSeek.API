using System;
using System.Collections.Generic;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class NotFoundResponse<TData> : Response<TData> where TData : class, IData
    {
        public NotFoundResponse()
        {
            Data = null;
            Errors = null;
        }
    }

    public class Response<TData> where TData : class, IData
    {
        public TData Data { get; protected set; }
        public List<Error> Errors { get; protected set; }
        public bool IsSuccessful { get { return Errors == null; } }

        protected Response()
        { }

        public Response(TData data)
        {
            Data = data;
        }

        public Response(ValidationException exception)
        {
            Errors = exception.Errors;
        }
    }
}