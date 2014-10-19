using System;
using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Commands
{
    public class CoachUpdateCommand : IBusinessIdable
    {
        public Guid BusinessId { get; set; }
        public Guid CoachId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public WeeklyWorkingHoursCommand WorkingHours { get; set; }


        public CoachData ToData()
        {
            return Mapper.Map<CoachUpdateCommand, CoachData>(this);
        }
    }
}