//using System;
//using CoachSeek.Domain;

//namespace CoachSeek.WebUI.Models
//{
//    public class Coach
//    {
//        public Guid Id { get; set; }

//        public string FirstName { get { return Person.FirstName; } }
//        public string LastName { get { return Person.LastName; } }
//        public string Name { get { return Person.Name; } }
//        public string Email { get { return EmailAddress.Email; } }
//        public string Phone { get { return PhoneNumber.Phone; }
//        }

//        private PersonName Person { get; set; }
//        private EmailAddress EmailAddress { get; set; }
//        private PhoneNumber PhoneNumber { get; set; }
        
//        public Coach(string firstName, string lastName, string email, string phone)
//        {
//            Id = Guid.NewGuid();
//            Person = new PersonName(firstName, lastName);
//            EmailAddress = new EmailAddress(email);
//            PhoneNumber = new PhoneNumber(phone);
//        }

//        public Coach(Guid id, string firstName, string lastName, string email, string phone)
//            : this(firstName, lastName, email, phone)
//        {
//            Id = id;
//        }
//    }
//}