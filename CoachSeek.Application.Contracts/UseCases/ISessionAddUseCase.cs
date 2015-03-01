using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ISessionAddUseCase
    {
        Guid BusinessId { set; }

        Response AddSession(SessionAddCommand command);
    }
}
