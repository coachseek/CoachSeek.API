using CoachSeek.WebUI.Models;
using CoachSeek.WebUI.Models.UseCases.Requests;

namespace CoachSeek.WebUI.Conversion
{
    public class CoachConverter
    {
        public static Coach Convert(CoachAddRequest request)
        {
            return new Coach
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone
            };
        }

        public static Coach Convert(CoachUpdateRequest request)
        {
            return new Coach
            {
                Id = request.CoachId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone
            };
        }
    }
}