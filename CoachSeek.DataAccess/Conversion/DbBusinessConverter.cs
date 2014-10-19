using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Models;
using CoachSeek.Domain.Entities;

namespace CoachSeek.DataAccess.Conversion
{
    public static class DbBusinessConverter
    {
        public static DbBusiness Convert(Business business)
        {
            var data = business.ToData();

            return Mapper.Map<BusinessData, DbBusiness>(data);
        }
    }
}