using System;
using CoachSeek.Application.Contracts.Models;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IEmailTemplateDeleteUseCase : IApplicationContextSetter
    {
        Response DeleteEmailTemplate(string templateType);
    }
}
