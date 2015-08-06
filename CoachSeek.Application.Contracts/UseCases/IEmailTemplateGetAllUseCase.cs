using System.Collections.Generic;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IEmailTemplateGetAllUseCase : IApplicationContextSetter
    {
        IList<EmailTemplateData> GetEmailTemplates();
    }
}
