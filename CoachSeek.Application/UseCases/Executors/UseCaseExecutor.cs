using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases.Executors;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Application.UseCases.Executors
{
    public class UseCaseExecutor : IUseCaseExecutor
    {
        public Response ExecuteFor<T>(T command, ApplicationContext context) where T : ICommand
        {
            if (command.GetType() == typeof(BookingSetAttendanceCommand))
            {
                var useCase = new BookingSetAttendanceUseCase();
                useCase.Initialise(context);
                return useCase.SetAttendance(command as BookingSetAttendanceCommand);
            }

            if (command.GetType() == typeof(BookingSetPaymentStatusCommand))
            {
                var useCase = new BookingSetPaymentStatusUseCase();
                useCase.Initialise(context);
                return useCase.SetPaymentStatus(command as BookingSetPaymentStatusCommand);
            }

            throw new NotImplementedException();
        }
    }
}