using System;
using CoachSeek.Application.Contracts.Models;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBookingDeleteUseCase : IBusinessRepositorySetter
    {
        Response DeleteBooking(Guid id);
    }
}
