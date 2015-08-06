using AutoMapper;
using CoachSeek.Api.Models.Api;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class EmailTemplateUpdateCommandConverter
    {
        public static EmailTemplateUpdateCommand Convert(ApiEmailTemplateSaveCommand apiEmailTemplateSaveCommand)
        {
            return Mapper.Map<ApiEmailTemplateSaveCommand, EmailTemplateUpdateCommand>(apiEmailTemplateSaveCommand);
        }
    }
}