using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class DiscountCodeAddCommandConverter
    {
        public static DiscountCodeAddCommand Convert(ApiDiscountCodeSaveCommand apiCommand)
        {
            return Mapper.Map<ApiDiscountCodeSaveCommand, DiscountCodeAddCommand>(apiCommand);
        }
    }
}