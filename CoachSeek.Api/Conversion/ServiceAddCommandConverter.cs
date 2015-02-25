using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public class ServiceAddCommandConverter
    {
        public static ServiceAddCommand Convert(ApiServiceSaveCommand apiCommand)
        {
            return Mapper.Map<ApiServiceSaveCommand, ServiceAddCommand>(apiCommand);
        }
    }
}