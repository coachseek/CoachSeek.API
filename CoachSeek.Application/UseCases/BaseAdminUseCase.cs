using CoachSeek.Application.Contracts;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public abstract class BaseAdminUseCase : IAdminApplicationContextSetter
    {
        protected AdminApplicationContext Context { set; get; }
        protected IUnsubscribedEmailAddressRepository UnsubscribedEmailAddressRepository { set; get; }


        public void Initialise(AdminApplicationContext context)
        {
            Context = context;

            UnsubscribedEmailAddressRepository = Context.EmailContext.UnsubscribedEmailAddressRepository;
        }


        protected ErrorResponse HandleException(CoachseekException ex)
        {
            if (ex is SingleErrorException)
                return new ErrorResponse(ex as SingleErrorException);
            if (ex is ValidationException)
                return new ErrorResponse(ex as ValidationException);

            throw ex;
        }
    }
}
