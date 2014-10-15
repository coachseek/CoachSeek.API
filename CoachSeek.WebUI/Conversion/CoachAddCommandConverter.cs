using CoachSeek.Domain.Commands;
using CoachSeek.WebUI.Models.Api;

namespace CoachSeek.WebUI.Conversion
{
    public static class CoachAddCommandConverter
    {
        public static CoachAddCommand Convert(ApiCoachSaveCommand apiCommand)
        {
            return new CoachAddCommand
            {
                BusinessId = apiCommand.BusinessId,
                FirstName = apiCommand.FirstName,
                LastName = apiCommand.LastName,
                Email = apiCommand.Email,
                Phone = apiCommand.Phone,
            };
        }
    }
}