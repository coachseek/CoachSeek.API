using CoachSeek.Application.Configuration;
using CoachSeek.Common;
using Coachseek.DataAccess.TableStorage.Authentication;
using CoachSeek.Domain.Entities;

namespace Coachseek.Tools.UpdateAzureUsersTableTool
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationAutoMapperConfigurator.Configure();
            var userRepository = new AzureTableUserRepository();
            var users = userRepository.GetAllAsync().Result;
            foreach (var user in users)
            {
                if (string.IsNullOrEmpty(user.Role))
                {
                    var data = user.ToData();
                    data.Role = Role.BusinessAdmin.ToString();
                    var updatedUser = new User(data);
                    updatedUser.SaveAsync(userRepository).Wait();
                }
            }
        }
    }
}
