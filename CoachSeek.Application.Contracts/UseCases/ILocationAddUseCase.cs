using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ILocationAddUseCase
    {
        Guid BusinessId { set; }

        Response AddLocation(LocationAddCommand command);
    }
}
