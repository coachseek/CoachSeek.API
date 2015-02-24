using System;
using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    public class Coach
    {
        public Guid Id { get; private set; }

        public string FirstName { get { return Person.FirstName; } }
        public string LastName { get { return Person.LastName; } }
        public string Name { get { return Person.Name; } }
        public string Email { get { return EmailAddress.Email; } }
        public string Phone { get { return PhoneNumber.Phone; } }
        public WeeklyWorkingHoursData WorkingHours { get { return WeeklyWorkingHours.ToData(); } }

        private PersonName Person { get; set; }
        private EmailAddress EmailAddress { get; set; }
        private PhoneNumber PhoneNumber { get; set; }
        private WeeklyWorkingHours WeeklyWorkingHours { get; set; }


        public Coach(Guid id, string firstName, string lastName, string email, string phone, WeeklyWorkingHoursCommand workingHoursCommand)
        {
            Id = id;
            Person = new PersonName(firstName, lastName);
            EmailAddress = new EmailAddress(email);
            PhoneNumber = new PhoneNumber(phone);
            WeeklyWorkingHours = new WeeklyWorkingHours(workingHoursCommand);
        }

        public Coach(Guid id, string firstName, string lastName, string email, string phone, WeeklyWorkingHoursData workingHoursData)
        {
            Id = id;
            Person = new PersonName(firstName, lastName);
            EmailAddress = new EmailAddress(email);
            PhoneNumber = new PhoneNumber(phone);
            WeeklyWorkingHours = new WeeklyWorkingHours(workingHoursData);
        }

        public Coach(CoachData data)
            : this(data.Id,
                   data.FirstName,
                   data.LastName,
                   data.Email,
                   data.Phone,
                   data.WorkingHours)
        { }

        public Coach(CoachUpdateCommand command)
            : this(command.Id,
                   command.FirstName,
                   command.LastName,
                   command.Email,
                   command.Phone,
                   command.WorkingHours)
        { }


        public CoachData ToData()
        {
            return Mapper.Map<Coach, CoachData>(this);
        }

        public CoachKeyData ToKeyData()
        {
            return Mapper.Map<Coach, CoachKeyData>(this);
        }
    }
}
