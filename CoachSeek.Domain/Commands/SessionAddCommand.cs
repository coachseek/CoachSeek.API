using System;
using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Commands
{
    public class SessionAddCommand : IBusinessIdable
    {
        public Guid BusinessId { get; set; }


        public NewSessionData ToData()
        {
            return Mapper.Map<SessionAddCommand, NewSessionData>(this);
        }
    }
}
