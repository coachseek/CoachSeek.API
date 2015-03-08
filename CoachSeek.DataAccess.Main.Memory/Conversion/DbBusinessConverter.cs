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
            return Mapper.Map<Business, DbBusiness>(business);
        }
    }
}