using System;
using CoachSeek.Domain.Data;

namespace CoachSeek.WebUI.Models.UseCases.Requests
{
    public class CoachAddRequest : Request
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }


        public NewCoachData ToData()
        {
            return new NewCoachData
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Phone = Phone
            };
        }
    }
}