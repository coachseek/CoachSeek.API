using System;
using CoachSeek.Application.Contracts;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Common;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public abstract class BaseUseCase : IApplicationContextSetter
    {
        protected ApplicationContext Context { set; get; }
        protected BusinessDetails Business { set; get; }
        protected CurrencyDetails Currency { set; get; }
        protected IBusinessRepository BusinessRepository { set; get; }
        protected ISupportedCurrencyRepository SupportedCurrencyRepository { set; get; }
        protected IUnsubscribedEmailAddressRepository UnsubscribedEmailAddressRepository { set; get; }


        public void Initialise(ApplicationContext context)
        {
            Context = context;

            Business = Context.BusinessContext.Business;
            Currency = Context.BusinessContext.Currency;

            BusinessRepository = Context.BusinessContext.BusinessRepository;
            SupportedCurrencyRepository = Context.BusinessContext.SupportedCurrencyRepository;
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
