using System;
using CoachSeek.Common;

namespace CoachSeek.Domain.Exceptions
{
    public class FirstNameRequired : SingleErrorException
    {
        public FirstNameRequired()
            : base(ErrorCodes.FirstNameRequired, "The FirstName field is required.")
        { }
    }
}
