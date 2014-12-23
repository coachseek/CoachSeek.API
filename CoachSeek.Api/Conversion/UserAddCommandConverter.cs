using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class UserAddCommandConverter
    {
        public static UserAddCommand Convert(ApiBusinessAdminCommand apiBusinessRegistrantCommand)
        {
            return Mapper.Map<ApiBusinessAdminCommand, UserAddCommand>(apiBusinessRegistrantCommand);
        }
    }
}