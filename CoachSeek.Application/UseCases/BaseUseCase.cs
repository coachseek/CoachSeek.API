using System;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases
{
    public abstract class BaseUseCase
    {
        public Guid BusinessId { set; protected get; }

        public IBusinessRepository BusinessRepository { set; protected get; }
    }
}
