using CoachSeek.Application.Contracts.Properties;
using CoachSeek.Common;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;
using System.Collections.Generic;

namespace CoachSeek.Application.Contracts.Models.Responses
{
    public class NoBusinessRegistrationDataResponse : BusinessRegistrationResponse
    {
        public NoBusinessRegistrationDataResponse()
        {
            Errors = new List<Error> { CreateNoBusinessRegistrationDataError() };
        }

        private static Error CreateNoBusinessRegistrationDataError()
        {
            return new Error((int)ErrorCodes.ErrorNoBusinessRegistrationData,
                             Resources.ErrorNoBusinessRegistrationData);
        }
    }
    public class DuplicateBusinessAdminBusinessRegistrationResponse : BusinessRegistrationResponse
    {
        public DuplicateBusinessAdminBusinessRegistrationResponse()
        {
            Errors = new List<Error> { CreateDuplicateBusinessAdminError() };
        }

        private static Error CreateDuplicateBusinessAdminError()
        {
            return new Error((int)ErrorCodes.ErrorBusinessAdminDuplicateEmail,
                             Resources.ErrorBusinessAdminDuplicateEmail,
                             FormField.Email);
        }
    }

    public class BusinessRegistrationResponse
    {
        public BusinessData Business { get; private set; }

        public List<Error> Errors { get; protected set; }


        public BusinessRegistrationResponse()
        { }

        public BusinessRegistrationResponse(BusinessData business)
        {
            Business = business;
        }

        public BusinessRegistrationResponse(ValidationException exception)
        {
            Errors = exception.Errors;
        }

        public bool IsSuccessful
        {
            get { return Business != null; }
        }
    }
}