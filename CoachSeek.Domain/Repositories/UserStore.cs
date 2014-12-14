using System.Threading.Tasks;
using CoachSeek.Domain.Entities;
using Microsoft.AspNet.Identity;

namespace CoachSeek.Domain.Repositories
{
    public class UserStore : IUserStore<User>
    {
        private IUserRepository UserRepository { get; set; }


        public UserStore(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }


        public async Task CreateAsync(User user)
        {
            await UserRepository.SaveAsync((NewUser)user);
        }

        public async Task DeleteAsync(User user)
        {
        }

        public async Task<User> FindByIdAsync(string userId)
        {
            return null;
        }

        public async Task<User> FindByNameAsync(string userName)
        {
            return UserRepository.GetByUsername(userName);
        }

        public async Task UpdateAsync(User user)
        {
        }

        public void Dispose()
        {
        }
    }
}