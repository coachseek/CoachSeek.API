using CoachSeek.WebUI.Models;
using CoachSeek.WebUI.Models.UseCases.Requests;

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
        public static Location Convert(LocationUpdateRequest request)
        {
            return new Location
            {
                Name = request.LocationName
            };
        }
    }
}