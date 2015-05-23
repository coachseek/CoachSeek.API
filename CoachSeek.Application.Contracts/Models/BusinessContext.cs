using System;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.Models
{
    public class BusinessContext
    {
        public Guid? BusinessId { get; private set; }
        public string BusinessName { get; private set; }
        public IBusinessRepository BusinessRepository { get; private set; }


        public BusinessContext(Guid? businessId, string businessName, IBusinessRepository businessRepository)
        {
            BusinessId = businessId;
            BusinessName = businessName;
            BusinessRepository = businessRepository;
        }
    }
}
