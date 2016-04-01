using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class DiscountCodeUpdateCommandConverter
    {
        public static DiscountCodeUpdateCommand Convert(ApiDiscountCodeSaveCommand apiCommand)
        {
            return Mapper.Map<ApiDiscountCodeSaveCommand, DiscountCodeUpdateCommand>(apiCommand);
        }
    }
}