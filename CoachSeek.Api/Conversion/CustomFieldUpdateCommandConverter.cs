using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class CustomFieldUpdateCommandConverter
    {
        public static CustomFieldUpdateCommand Convert(ApiCustomFieldSaveCommand apiCommand)
        {
            apiCommand.Type = apiCommand.Type.ToLowerInvariant();

            return Mapper.Map<ApiCustomFieldSaveCommand, CustomFieldUpdateCommand>(apiCommand);
        }
    }
}