using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IServiceAddUseCase : IApplicationContextSetter
    {
        IResponse AddService(ServiceAddCommand command);
    }
}
