using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class BusinessUpdateCommandConverter
    {
        public static BusinessUpdateCommand Convert(ApiBusinessSaveCommand apiSaveBusinessCommand)
        {
            return Mapper.Map<ApiBusinessSaveCommand, BusinessUpdateCommand>(apiSaveBusinessCommand);
        }
    }
}