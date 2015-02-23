using CoachSeek.Domain.Commands;
using CoachSeek.Services.Contracts.Builders;

namespace CoachSeek.Domain.Entities
{
    public class NewBusiness2 : Business2
    {
        public NewBusiness2(BusinessAddCommand command, IBusinessDomainBuilder domainBuilder)
        {
            // Id is set via base class
            Name = command.Name.Trim();
            Domain = domainBuilder.BuildDomain(command.Name);
        }
    }
}