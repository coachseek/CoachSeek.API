using CoachSeek.Data.Model;

namespace CoachSeek.Application.Contracts.UseCases
{
    public interface IEmailTemplateGetByTypeUseCase : IApplicationContextSetter
    {
        EmailTemplateData GetEmailTemplate(string templateType);
    }
}
