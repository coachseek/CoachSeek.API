﻿using System.Threading.Tasks;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface ICustomFieldTemplateUpdateUseCase : IApplicationContextSetter
    {
        Task<IResponse> UpdateCustomFieldTemplateAsync(CustomFieldUpdateCommand command);
    }
}
