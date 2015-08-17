using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Api.Filters
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errors = RepackageErrors(actionContext);

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errors);
            }
        }

        private static List<Error> RepackageErrors(HttpActionContext actionContext)
        {
            var errors = new ValidationException();
            foreach (var key in actionContext.ModelState.Keys)
                foreach (var error in actionContext.ModelState[key].Errors)
                    errors.Add(CreateModelStateException(key, error.ErrorMessage));

            return errors.Errors;
        }

        private static SingleErrorException CreateModelStateException(string key, string errorMessage)
        {
            var fieldName = ExtractFieldName(key);
            var fieldQualifier = CreateFieldQualifier(errorMessage);
            var errorCode = string.Format("{0}-{1}", fieldName, fieldQualifier);
            return new SingleErrorException(errorCode, errorMessage);
        }

        private static string ExtractFieldName(string componentField)
        {
            var componentFields = componentField.Split('.');
            var field = componentFields[componentFields.GetUpperBound(0)];
            return field.ToLowerInvariant();
        }

        private static string CreateFieldQualifier(string errorMessage)
        {
            if (errorMessage.EndsWith("is required."))
                return "required";
            if (errorMessage.Contains("a maximum length of"))
                return "too-long";
            if (errorMessage.Contains("is not a valid"))
                return "invalid";

            return null;
        }

        //private static SingleErrorException CreateModelStateException(string key, string errorMessage)
        //{
        //    if (key.EndsWithIgnoreCase("Business") && (errorMessage.CompareIgnoreCase("The Business field is required.")))
        //        return (new BusinessDetailsMissing(errorMessage));
        //    if (key.EndsWithIgnoreCase("Business.Name") && (errorMessage.CompareIgnoreCase("The Name field is required.")))
        //        return (new BusinessNameMissing(errorMessage));
        //    if (key.EndsWithIgnoreCase("Currency") && (errorMessage.CompareIgnoreCase("The Currency field is required.")))
        //        return (new CurrencyMissing(errorMessage));
        //    if (key.EndsWithIgnoreCase("Currency") && (errorMessage.CompareIgnoreCase("The field Currency must be a string with a maximum length of 3.")))
        //        return (new CurrencyTooLong(errorMessage));
        //    if (key.EndsWithIgnoreCase("Payment") && (errorMessage.CompareIgnoreCase("The Payment field is required.")))
        //        return (new PaymentDetailsMissing(errorMessage));
        //    if (key.EndsWithIgnoreCase("Admin") && (errorMessage.CompareIgnoreCase("The Admin field is required.")))
        //        return (new UserDetailsMissing(errorMessage));
        //    if (key.EndsWithIgnoreCase("FirstName") && (errorMessage.CompareIgnoreCase("The FirstName field is required.")))
        //        return (new FirstNameMissing(errorMessage));
        //    if (key.EndsWithIgnoreCase("LastName") && (errorMessage.CompareIgnoreCase("The LastName field is required.")))
        //        return (new LastNameMissing(errorMessage));
        //    if (key.EndsWithIgnoreCase("Email") && (errorMessage.CompareIgnoreCase("The Email field is required.")))
        //        return (new EmailAddressMissing(errorMessage));
        //    if (key.EndsWithIgnoreCase("Email") && (errorMessage.CompareIgnoreCase("The field Email must be a string with a maximum length of 100.")))
        //        return (new EmailAddressTooLong(errorMessage));
        //    if (key.EndsWithIgnoreCase("Email") && (errorMessage.CompareIgnoreCase("The Email field is not a valid e-mail address.")))
        //        return (new EmailAddressFormatInvalid(errorMessage));
        //    if (key.EndsWithIgnoreCase("Password") && (errorMessage.CompareIgnoreCase("The Password field is required.")))
        //        return (new UserPasswordMissing(errorMessage));
        //    if (key.EndsWithIgnoreCase("Password") && (errorMessage.CompareIgnoreCase("The field Password must be a string with a maximum length of 20.")))
        //        return (new UserPasswordTooLong(errorMessage));
        //    if (key.EndsWithIgnoreCase("booking.Sessions") && (errorMessage.CompareIgnoreCase("The Sessions field is required.")))
        //        return (new BookingSessionsMissing(errorMessage));
        //    if (key.EndsWithIgnoreCase("booking.Customer") && (errorMessage.CompareIgnoreCase("The Customer field is required.")))
        //        return (new BookingCustomerMissing(errorMessage));

        //    throw new NotImplementedException("Unable to resolve to Model State Exception.");
        //}
    }
}