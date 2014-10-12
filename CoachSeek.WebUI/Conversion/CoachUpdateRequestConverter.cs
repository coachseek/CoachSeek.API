using System;
using CoachSeek.Domain.Commands;
using CoachSeek.WebUI.Models.Api;

namespace CoachSeek.WebUI.Conversion
{
    public static class CoachUpdateRequestConverter
    {
        public static CoachUpdateRequest Convert(ApiCoachSaveRequest apiRequest)
        {
            return new CoachUpdateRequest
            {
                CoachId = apiRequest.Id.HasValue ? apiRequest.Id.Value : Guid.Empty,
                BusinessId = apiRequest.BusinessId,
                FirstName = apiRequest.FirstName,
                LastName = apiRequest.LastName,
                Email = apiRequest.Email,
                Phone = apiRequest.Phone,
            };
        }
    }
}