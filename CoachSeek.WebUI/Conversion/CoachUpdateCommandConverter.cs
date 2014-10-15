using System;
using CoachSeek.Domain.Commands;
using CoachSeek.WebUI.Models.Api;

namespace CoachSeek.WebUI.Conversion
{
    public static class CoachUpdateCommandConverter
    {
        public static CoachUpdateCommand Convert(ApiCoachSaveCommand apiCommand)
        {
            return new CoachUpdateCommand
            {
                CoachId = apiCommand.Id.HasValue ? apiCommand.Id.Value : Guid.Empty,
                BusinessId = apiCommand.BusinessId,
                FirstName = apiCommand.FirstName,
                LastName = apiCommand.LastName,
                Email = apiCommand.Email,
                Phone = apiCommand.Phone,
            };
        }
    }
}