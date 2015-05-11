using CoachSeek.Application.Contracts.Models;

namespace CoachSeek.Application.Contracts
{
    public interface IApplicationContextSetter
    {
        void Initialise(ApplicationContext context);
    }
}
