using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Models;

namespace CoachSeek.DataAccess.Main.Memory.Conversion
{
    public static class DbBusinessConverter
    {
        public static DbBusiness Convert(Domain.Entities.Business business)
        {
            var data = business.ToData();

            return Mapper.Map<BusinessData, DbBusiness>(data);
        }
    }
}