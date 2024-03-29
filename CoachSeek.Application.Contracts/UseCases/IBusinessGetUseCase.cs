﻿using System.Threading.Tasks;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IBusinessGetUseCase : IApplicationContextSetter
    {
        Task<BusinessData> GetBusinessAsync();
    }
}
