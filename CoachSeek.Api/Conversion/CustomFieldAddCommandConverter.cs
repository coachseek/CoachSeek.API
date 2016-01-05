using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class CustomFieldAddCommandConverter
    {
        public static CustomFieldAddCommand Convert(ApiCustomFieldSaveCommand apiCommand)
        {
            apiCommand.Type = apiCommand.Type.ToLowerInvariant();

            return Mapper.Map<ApiCustomFieldSaveCommand, CustomFieldAddCommand>(apiCommand);
        }
    }
}