using System;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Contracts;
using Newtonsoft.Json.Linq;

namespace CoachSeek.Api.Conversion
{
    public class DomainCommandConverter
    {
        public static ICommand Convert(dynamic apiCommand)
        {
            string commandName = apiCommand.commandName;

            if (commandName == "BusinessSetAuthorisedUntil")
                return ConvertToConcreteCommand<BusinessSetAuthorisedUntilCommand>(apiCommand);

            if (commandName == "BookingSetAttendance")
                return ConvertToConcreteCommand<BookingSetAttendanceCommand>(apiCommand);
            if (commandName == "BookingSetPaymentStatus")
                return ConvertToConcreteCommand<BookingSetPaymentStatusCommand>(apiCommand);
            
            throw new NotImplementedException();
        }

        private static T ConvertToConcreteCommand<T>(JToken command)
        {
            return command.ToObject<T>();
        }
    }
}