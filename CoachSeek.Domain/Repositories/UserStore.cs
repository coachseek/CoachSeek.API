//using System.Threading.Tasks;
//using CoachSeek.Data.Model;
//using CoachSeek.Domain.Entities;
//using Microsoft.AspNet.Identity;

//namespace CoachSeek.Domain.Repositories
//{
//    public class UserStore : IUserStore<User>, IUserPasswordStore<User>
//    {
//        private IUserRepository UserRepository { get; set; }


//        public UserStore(IUserRepository userRepository)
//        {
//            UserRepository = userRepository;
//        }


//        public async Task CreateAsync(User user)
//        {
//            UserRepository.Save((NewUser)user);
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
//            //var user = UserRepository.GetByUsername(userName);

//            // Test code
//            var user = new User(new UserData {Username = userName, Email = "a@b.c", FirstName = "a", LastName = "b", Password = "xxx"});
//            // Test code

//            return user;
//        }

//        public async Task UpdateAsync(User user)
//        {
//        }

//        public void Dispose()
//        {
//        }

//        public async Task<string> GetPasswordHashAsync(User user)
//        {
//            return user.Password;
//        }

//        public async Task<bool> HasPasswordAsync(User user)
//        {
//            return !string.IsNullOrEmpty(user.Password);
//        }

//        public async Task SetPasswordHashAsync(User user, string passwordHash)
//        {
//            user.Password = passwordHash;
//        }
//    }
//}