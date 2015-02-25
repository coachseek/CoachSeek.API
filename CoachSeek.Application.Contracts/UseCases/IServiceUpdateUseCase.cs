using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IServiceUpdateUseCase
    {
        Guid BusinessId { set; }

        Response UpdateService(ServiceUpdateCommand command);
    }
}
