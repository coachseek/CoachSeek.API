using AutoMapper;
using CoachSeek.Domain.Commands;
using CoachSeek.WebUI.Models.Api;

namespace CoachSeek.WebUI.Conversion
{
    public class ServiceAddCommandConverter
    {
        public static ServiceAddCommand Convert(ApiServiceSaveCommand apiCommand)
        {
            return Mapper.Map<ApiServiceSaveCommand, ServiceAddCommand>(apiCommand);
        }
    }
}