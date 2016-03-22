﻿namespace CoachSeek.Domain.Commands
{
    public class CustomerAddCommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Sex { get; set; }
        public string DateOfBirth { get; set; }


        public CustomerAddCommand()
        { }

        public CustomerAddCommand(string firstName, string lastName, string email, string sex, string phone)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Sex = sex;
            Phone = phone;
        }
    }
}