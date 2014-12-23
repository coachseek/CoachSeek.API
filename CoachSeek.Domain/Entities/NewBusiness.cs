using System;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using CoachSeek.Services.Contracts.Builders;

namespace CoachSeek.Domain.Entities
{
    public class NewBusiness : Business
    {
        public NewBusiness(BusinessAddCommand command, IBusinessDomainBuilder domainBuilder)
        {
            // Id is set via base class
            Name = command.Name.Trim();
            Domain = domainBuilder.BuildDomain(command.Name);
        }



        public BusinessData Register(IBusinessRepository repository)
        {
            Validate(repository);
            repository.Save(this);

            return ToData();
        }


        private void Validate(IBusinessRepository repository)
        {
        }


        //private class NewBusinessAdmin : BusinessAdmin
        //{
        //    public NewBusinessAdmin(BusinessRegistrantCommand registrant)
        //        : this(registrant.FirstName, registrant.LastName, registrant.Email, registrant.Password)
        //    { }

        //    private NewBusinessAdmin(string firstName, string lastName, string email, string password)
        //    {
        //        Id = Guid.NewGuid();
        //        Person = new PersonName(firstName, lastName);
        //        EmailAddress = new EmailAddress(email);
        //        // Email is also the Username.
        //        Credential = new Credential(email, password);
        //    }


        //    public void Validate(IBusinessRepository repository)
        //    {
        //        var admin = repository.GetByAdminEmail(Email);
        //        if (admin.IsExisting())
        //            throw new DuplicateBusinessAdmin();
        //    }
        //}
    }
}