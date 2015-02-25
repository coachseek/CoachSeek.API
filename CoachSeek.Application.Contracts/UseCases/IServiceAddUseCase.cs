using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IServiceAddUseCase
    {
        Guid BusinessId { set; }

        Response AddService(ServiceAddCommand command);
    }
}
