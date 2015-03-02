using System;
using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Commands
{
    public class CoachUpdateCommand : CoachAddCommand, IIdentifiable
    {
        public Guid Id { get; set; }


        public CoachData ToData()
        {
            return Mapper.Map<CoachUpdateCommand, CoachData>(this);
        }
    }
}