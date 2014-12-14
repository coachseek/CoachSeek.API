using System.Threading.Tasks;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;
using Microsoft.AspNet.Identity;

namespace CoachSeek.Domain.Entities
{
    public class NewUser : User
    {
        public NewUser(UserAddCommand command)
            : this(command.FirstName, command.LastName, command.Email, command.Password)
        { }

        private NewUser(string firstName, string lastName, string email, string password)
        {
            Id = System.Guid.NewGuid().ToString();
            Person = new PersonName(firstName, lastName);
            EmailAddress = new EmailAddress(email);
            // Email is also the Username.
            Credential = new Credential(email, password);
        }


        public async Task<UserData> Save(IUserRepository repository)
        {
            var userManager = CreateUserManager(new UserStore(repository));

            var user = await userManager.FindByNameAsync(Email);
            if (user.IsExisting())
                throw new DuplicateUser();

            await userManager.CreateAsync(this);

            return ToData();
        }


        private async void Validate(UserManager<User, string> userManager)
        {
            var user = await userManager.FindByNameAsync(Email);
            if (user.IsExisting())
                throw new DuplicateUser();
        }

        private UserManager<User> CreateUserManager(IUserStore<User> userStore)
        {
            return new UserManager<User>(userStore);
        }
    }
}
