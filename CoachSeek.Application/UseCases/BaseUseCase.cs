using System;
using CoachSeek.Application.Contracts;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public abstract class BaseUseCase : IApplicationContextSetter
    {
        protected ApplicationContext Context { set; get; }
        protected Guid BusinessId { set; get; }
        protected IBusinessRepository BusinessRepository { set; get; }


        public void Initialise(ApplicationContext context)
        {
            Context = context;

            BusinessId = Context.BusinessId.HasValue ? Context.BusinessId.Value : Guid.Empty;
            BusinessRepository = Context.BusinessRepository;
        }
    }
}
