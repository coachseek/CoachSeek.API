using AutoMapper;
using CoachSeek.Domain.Commands;
using CoachSeek.WebUI.Models.Api;
using CoachSeek.WebUI.Models.Api.Setup;

namespace CoachSeek.WebUI.Conversion
{
    public static class CoachUpdateCommandConverter
    {
        public static CoachUpdateCommand Convert(ApiCoachSaveCommand apiCommand)
        {
            return Mapper.Map<ApiCoachSaveCommand, CoachUpdateCommand>(apiCommand);

            //return new CoachUpdateCommand
            //{
            //    CoachId = apiCommand.Id.HasValue ? apiCommand.Id.Value : Guid.Empty,
            //    BusinessId = apiCommand.BusinessId.Value,
            //    FirstName = apiCommand.FirstName,
            //    LastName = apiCommand.LastName,
            //    Email = apiCommand.Email,
            //    Phone = apiCommand.Phone,
            //};
        }
    }
}