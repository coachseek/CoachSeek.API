using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
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
            var newBusiness = repository.Save(this);

            return newBusiness.ToData();
        }
    }
}