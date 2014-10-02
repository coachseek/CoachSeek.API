using CoachSeek.WebUI.Models;
using CoachSeek.WebUI.Models.Requests;

namespace CoachSeek.WebUI.Conversion
{
    public class LocationConverter
    {
        public static Location Convert(LocationAddRequest request)
        {
            return new Location
            {
                Name = request.LocationName
            };
        }
    }
}