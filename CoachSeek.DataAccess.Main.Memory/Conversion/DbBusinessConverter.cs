using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Main.Memory.Models;
using CoachSeek.Domain.Entities;

namespace CoachSeek.DataAccess.Main.Memory.Conversion
{
    public static class DbBusinessConverter
    {
        public static DbBusiness Convert(Business business)
        {
            var data = business.ToData();

            return Mapper.Map<BusinessData, DbBusiness>(data);
        }

        public static DbBusiness Convert(Business2 business)
        {
            return Mapper.Map<Business2, DbBusiness>(business);
        }
    }
}