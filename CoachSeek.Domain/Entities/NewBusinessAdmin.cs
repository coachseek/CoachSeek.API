//using CoachSeek.Common.Extensions;
//using CoachSeek.Domain.Commands;
//using CoachSeek.Domain.Exceptions;
//using CoachSeek.Domain.Repositories;
//using System;

//namespace CoachSeek.Domain.Entities
//{
//    public class NewBusinessAdmin : BusinessAdmin
//    {
//        public NewBusinessAdmin(BusinessRegistrant registrant)
//            : this(registrant.FirstName, registrant.LastName, registrant.Email, registrant.Password)
//        { }

//        private NewBusinessAdmin(string firstName, string lastName, string email, string password)
//        {
//            Id = Guid.NewGuid();
//            Person = new PersonName(firstName, lastName);
//            EmailAddress = new EmailAddress(email);
//            // Email is also the Username.
//            Credential = new Credential(email, password);
//        }


//        public void Validate(IBusinessRepository repository)
//        {
//            var admin = repository.GetByAdminEmail(Email);
//            if (admin.IsExisting())
//                throw new DuplicateBusinessAdmin();
//        }
//    }
//}
