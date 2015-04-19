using System;
using CoachSeek.Api.Models.Api.Out;
using CoachSeek.Data.Model;

namespace CoachSeek.Api.Conversion.Out
{
    public static class ApiOutSessionConverter
    {
        public static ApiOutSession Convert(SessionData sessionData)
        {
            if (sessionData == null)
                return null;

            if (sessionData is SingleSessionData)
                return ApiOutSingleSessionConverter.Convert((SingleSessionData)sessionData);

            if (sessionData is RepeatedSessionData)
                return ApiOutCourseConverter.Convert((RepeatedSessionData)sessionData);

            throw new InvalidOperationException("Unexpected SessionData type.");
        }
    }
}