using CoachSeek.WebUI.Models;
using System;
using System.Collections.Generic;

namespace CoachSeek.WebUI.Exceptions
{
    public class ValidationException : Exception
    {
        public List<Error> Errors { get; private set; }


        public ValidationException(int errorCode, string errorMessage)
        {
            Errors = new List<Error> {new Error(errorCode, errorMessage)};
        }

        public ValidationException(Error error)
        {
            Errors = new List<Error> { error };
        }

        public ValidationException(IEnumerable<Error> errors)
        {
            Errors = new List<Error>(errors);
        }
    }
}