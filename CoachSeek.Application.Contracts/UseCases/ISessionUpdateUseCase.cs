using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ISessionUpdateUseCase
    {
        Guid BusinessId { set; }

        Response UpdateSession(SessionUpdateCommand command);
    }
}
