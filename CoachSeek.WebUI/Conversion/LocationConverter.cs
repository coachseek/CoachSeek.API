using CoachSeek.Domain.Entities;
using CoachSeek.WebUI.Models.UseCases.Requests;

namespace CoachSeek.WebUI.Conversion
{
    public static class LocationConverter
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
                Id = request.LocationId,
                Name = request.LocationName
            };
        }
    }
}