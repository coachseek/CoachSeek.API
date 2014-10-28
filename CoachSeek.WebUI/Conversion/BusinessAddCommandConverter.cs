﻿using AutoMapper;
using CoachSeek.Domain.Commands;
using CoachSeek.WebUI.Models.Api;

namespace CoachSeek.WebUI.Conversion
{
    public static class BusinessAddCommandConverter
    {
        public static BusinessRegistrationCommand Convert(ApiBusinessRegistrationCommand apiBusinessRegistrationCommand)
        {
            return Mapper.Map<ApiBusinessRegistrationCommand, BusinessRegistrationCommand>(apiBusinessRegistrationCommand);
        }
    }
}