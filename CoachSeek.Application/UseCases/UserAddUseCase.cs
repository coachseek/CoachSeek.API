using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public class UserAddUseCase : IUserAddUseCase
    {
        public IUserRepository UserRepository { get; set; }


        public async Task<IResponse> AddUserAsync(UserAddCommand command)
        {
            var newUser = new NewUser(command);
            await newUser.SaveAsync(UserRepository);
            return new Response(newUser.ToData());
        }
    }
}
