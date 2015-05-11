using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IServiceAddUseCase : IApplicationContextSetter
    {
        Response AddService(ServiceAddCommand command);
    }
}
