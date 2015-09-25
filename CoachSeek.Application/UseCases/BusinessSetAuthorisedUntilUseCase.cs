using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.UseCases
{
    public class BusinessSetAuthorisedUntilUseCase : BaseAdminUseCase, IBusinessSetAuthorisedUntilUseCase
    {
        public async Task<IResponse> SetAuthorisedUntilAsync(BusinessSetAuthorisedUntilCommand command)
        {
            var business = await Context.BusinessRepository.GetBusinessAsync(command.BusinessId);
            if (business.IsNotFound())
                return new NotFoundResponse();
            await Context.BusinessRepository.SetAuthorisedUntilAsync(command.BusinessId, command.AuthorisedUntil);
            return new Response();
        }
    }
}