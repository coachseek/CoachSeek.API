using AutoMapper;
using CoachSeek.Data.Model;
using System;

namespace CoachSeek.Domain.Commands
{
    public class SessionUpdateCommand : SessionAddCommand, IIdentifiable
    {
        public Guid Id { get; set; }


        public new SessionData ToData()
        {
            return Mapper.Map<SessionUpdateCommand, SessionData>(this);
        }
    }
}
