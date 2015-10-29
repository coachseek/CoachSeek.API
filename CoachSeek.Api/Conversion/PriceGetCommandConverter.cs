using AutoMapper;
using CoachSeek.Api.Models.Api;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class PriceGetCommandConverter
    {
        public static PriceGetCommand Convert(ApiPriceGetCommand command)
        {
            return Mapper.Map<ApiPriceGetCommand, PriceGetCommand>(command);
        }
    }
}