using CoachSeek.Application.Contracts.Models;

namespace CoachSeek.Application.Contracts
{
    public interface IAdminApplicationContextSetter
    {
        void Initialise(AdminApplicationContext context);
    }
}
