using CoachSeek.WebUI.Models;

namespace CoachSeek.WebUI.Conversion
{
    public class BusinessAdminConverter
    {
        public static BusinessAdmin Convert(BusinessRegistrant registrant, int businessId)
        {
            return new BusinessAdmin
            {
                BusinessId = businessId,
                FirstName = registrant.FirstName,
                LastName = registrant.LastName,
                Email = registrant.Email,
                Password = registrant.Password,
            };
        }
    }
}