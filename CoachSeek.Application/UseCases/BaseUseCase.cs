using CoachSeek.Application.Contracts;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public abstract class BaseUseCase : IApplicationContextSetter
    {
        protected ApplicationContext Context { set; get; }
        protected Business Business { set; get; }
        protected IBusinessRepository BusinessRepository { set; get; }
        protected ISupportedCurrencyRepository SupportedCurrencyRepository { set; get; }
        protected IUnsubscribedEmailAddressRepository UnsubscribedEmailAddressRepository { set; get; }
        protected ILogRepository LogRepository { set; get; }


        public void Initialise(ApplicationContext context)
        {
            Context = context;

            Business = Context.BusinessContext.Business;

            BusinessRepository = Context.BusinessContext.BusinessRepository;
            SupportedCurrencyRepository = Context.BusinessContext.SupportedCurrencyRepository;
            if (Context.EmailContext.IsExisting())
                UnsubscribedEmailAddressRepository = Context.EmailContext.UnsubscribedEmailAddressRepository;
            LogRepository = Context.LogRepository;
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
