using System;
using AutoMapper;
using CoachSeek.Data.Model;

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

        private PersonName Person { get; set; }
        private EmailAddress EmailAddress { get; set; }
        private PhoneNumber PhoneNumber { get; set; }
        private WeeklyWorkingHours WorkingHours { get; set; }


        public Coach(Guid id, string firstName, string lastName, string email, string phone, WeeklyWorkingHoursData workingHoursData)
        {
            Id = id;
            Person = new PersonName(firstName, lastName);
            EmailAddress = new EmailAddress(email);
            PhoneNumber = new PhoneNumber(phone);
            WorkingHours = new WeeklyWorkingHours(workingHoursData);
        }

        public Coach(CoachData data)
            : this(data.Id, 
                   data.FirstName, 
                   data.LastName, 
                   data.Email, 
                   data.Phone,
                   data.WorkingHours)
        { }


        public CoachData ToData()
        {
            return Mapper.Map<Coach, CoachData>(this);
        }
    }
}
