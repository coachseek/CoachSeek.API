using CoachSeek.WebUI.Models;
using CoachSeek.WebUI.Models.Requests;

namespace CoachSeek.WebUI.Conversion
{
    public static class BusinessConverter
    {
        public static Business Convert(BusinessRegistrationRequest registration)
        {
            return new Business
            {
                Name = registration.BusinessName
            };
        }
    }
}