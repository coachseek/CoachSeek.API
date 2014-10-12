using CoachSeek.Domain.Commands;
using CoachSeek.WebUI.Models.Api;

namespace CoachSeek.WebUI.Conversion
{
    public static class CoachAddRequestConverter
    {
        public static CoachAddRequest Convert(ApiCoachSaveRequest coachSaveRequest)
        {
            return new CoachAddRequest
            {
                BusinessId = coachSaveRequest.BusinessId,
                FirstName = coachSaveRequest.FirstName,
                LastName = coachSaveRequest.LastName,
                Email = coachSaveRequest.Email,
                Phone = coachSaveRequest.Phone,
            };
        }
    }
}