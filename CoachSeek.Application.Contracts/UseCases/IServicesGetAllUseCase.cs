using System;
using System.Collections.Generic;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IServicesGetAllUseCase
    {
        Guid BusinessId { get; set; }

        IList<ServiceData> GetServices();
    }
}
