using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IUserRepositorySetter
    {
        IUserRepository UserRepository { set; }
    }
}
