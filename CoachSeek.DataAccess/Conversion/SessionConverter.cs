using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Models;

namespace CoachSeek.DataAccess.Conversion
{
    public class SessionConverter : ITypeConverter<SessionData, DbSession>
    {
        public DbSession Convert(ResolutionContext context)
        {
            return Mapper.DynamicMap<DbSession>(context.SourceValue);
        }
    }
}
