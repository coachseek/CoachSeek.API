using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class BusinessRegistrationCommandConverter
    {
        public static BusinessRegistrationCommand Convert(ApiBusinessRegistrationCommand apiCommand)
        {
            return new BusinessRegistrationCommand
            {
                Admin = UserAddCommandConverter.Convert(apiCommand.Admin),
                Business = BusinessAddCommandConverter.Convert(apiCommand.Business)
            };
        }
    }
}