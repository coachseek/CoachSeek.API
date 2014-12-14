//using System.Threading.Tasks;
//using CoachSeek.Domain.Entities;
//using CoachSeek.Domain.Repositories;
//using Microsoft.AspNet.Identity;

//namespace CoachSeek.DataAccess.Authentication.Alias
//{
//    public class UserStore : IUserStore<User>
//    {
//        private IUserRepository UserRepository { get; set; }


//        public UserStore(IUserRepository userRepository)
//        {
//            UserRepository = userRepository;
//        }


//        public async Task CreateAsync(User user)
//        {
//        }

//        public async Task DeleteAsync(User user)
//        {
//        }

//        public async Task<User> FindByIdAsync(string userId)
//        {
//            return null;
//        }

//        public async Task<User> FindByNameAsync(string userName)
//        {
//            return null;
//        }

//        public async Task UpdateAsync(User user)
//        {
//        }

//        public void Dispose()
//        {
//        }
//    }
//}