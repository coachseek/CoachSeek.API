using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.Models
{
    public class UserContext
    {
        //private User User { get; set; }
        public IUserRepository UserRepository { get; private set; }


        public UserContext(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        //public UserContext(User currentUser, IUserRepository userRepository)
        //{
        //    User = currentUser;
        //    UserRepository = userRepository;
        //}
    }
}
