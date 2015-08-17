using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Factories;

namespace CoachSeek.Application.UseCases
{
    public class EmailTemplateUpdateUseCase : BaseUseCase, IEmailTemplateUpdateUseCase
    {
        public IResponse UpdateEmailTemplate(EmailTemplateUpdateCommand command)
        {
            try
            {
                var template = EmailTemplateFactory.BuildEmailTemplate(command);
                var existingTemplate = BusinessRepository.GetEmailTemplate(Business.Id, template.Type);
                if (existingTemplate.IsNotFound())
                    BusinessRepository.AddEmailTemplate(Business.Id, template);
                else
                    BusinessRepository.UpdateEmailTemplate(Business.Id, template);
                return new Response();
            }
            catch (CoachseekException ex)
            {
                return HandleException(ex);
            }
        }
    }
}
