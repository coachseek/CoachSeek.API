using AutoMapper;
using CoachSeek.Api.Models.Api.Out;
using CoachSeek.Data.Model;

namespace CoachSeek.Api.Conversion.Out
{
    public static class ApiOutBusinessConverter
    {
        public static ApiOutBasicBusiness ConvertToBasicBusiness(BusinessData business)
        {
            return Mapper.Map<BusinessData, ApiOutBasicBusiness>(business);
        }

        public static ApiOutBusiness ConvertToBusiness(BusinessData business)
        {
            return Mapper.Map<BusinessData, ApiOutBusiness>(business);
        }
    }
}