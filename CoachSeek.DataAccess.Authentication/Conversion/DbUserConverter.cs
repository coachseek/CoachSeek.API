using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Models;
using CoachSeek.Domain.Entities;

namespace CoachSeek.DataAccess.Authentication.Conversion
{
    public static class DbUserConverter
    {
        public static DbUser Convert(User user)
        {
            var data = user.ToData();

            return Mapper.Map<UserData, DbUser>(data);
        }
    }
}
