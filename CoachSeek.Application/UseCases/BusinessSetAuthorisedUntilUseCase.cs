using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.UseCases
{
    public class BusinessSetAuthorisedUntilUseCase : BaseAdminUseCase, IBusinessSetAuthorisedUntilUseCase
    {
        public IResponse SetAuthorisedUntil(BusinessSetAuthorisedUntilCommand command)
        {
            var business = Context.BusinessRepository.GetBusiness(command.BusinessId);
            if (business.IsNotFound())
                return new NotFoundResponse();
            Context.BusinessRepository.SetAuthorisedUntil(command.BusinessId, command.AuthorisedUntil);
            return new Response();
        }
    }
}