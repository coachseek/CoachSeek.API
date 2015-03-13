﻿using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBookingGetByIdUseCase : IBusinessRepositorySetter
    {
        BookingData GetBooking(Guid id);
    }
}
