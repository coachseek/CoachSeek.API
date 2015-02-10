using CoachSeek.Domain.Entities;
using System;
using CoachSeek.Domain.Entities.Booking;

namespace CoachSeek.Domain.Repositories
{
    public interface IBookingRepository
    {
        //Business Save(NewBusiness newBusiness);
        //Business Save(Business business);
        //Business Get(Guid id);
        //Business GetByDomain(string domain);

        Booking SaveNew(Guid businessId, Booking booking);
        Booking SaveExisting(Guid businessId, Booking booking);
        Booking Get(Guid businessId, Guid id);
    }
}
