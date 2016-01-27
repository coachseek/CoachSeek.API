using System;
using AutoMapper;
using CoachSeek.Api.Models.Api.Setup;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class CustomFieldValuesUpdateCommandConverter
    {
        public static CustomFieldValueListUpdateCommand Convert(string type, Guid typeId, ApiCustomFieldValueListSaveCommand apiCommand)
        {
            var command = Mapper.Map<ApiCustomFieldValueListSaveCommand, CustomFieldValueListUpdateCommand>(apiCommand);
            command.Type = type;
            command.TypeId = typeId;

            return command;
        }
    }
}