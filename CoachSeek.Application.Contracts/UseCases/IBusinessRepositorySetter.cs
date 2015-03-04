using System;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBusinessRepositorySetter
    {
        Guid BusinessId { set; }

        IBusinessRepository BusinessRepository { set; }
    }
}
