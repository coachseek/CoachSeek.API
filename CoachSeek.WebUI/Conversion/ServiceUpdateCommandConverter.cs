using AutoMapper;
using CoachSeek.Domain.Commands;
using CoachSeek.WebUI.Models.Api;

namespace CoachSeek.WebUI.Conversion
{
    public class ServiceUpdateCommandConverter
    {
        public static ServiceUpdateCommand Convert(ApiServiceSaveCommand apiCommand)
        {
            return Mapper.Map<ApiServiceSaveCommand, ServiceUpdateCommand>(apiCommand);
        }
    }
}