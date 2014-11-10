using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public class ServiceUpdateCommandConverter
    {
        public static ServiceUpdateCommand Convert(ApiServiceSaveCommand apiCommand)
        {
            return Mapper.Map<ApiServiceSaveCommand, ServiceUpdateCommand>(apiCommand);
        }
    }
}