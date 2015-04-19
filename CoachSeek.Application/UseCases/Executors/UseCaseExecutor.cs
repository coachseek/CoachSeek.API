using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases.Executors;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Contracts;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Application.UseCases.Executors
{
    public class UseCaseExecutor : IUseCaseExecutor
    {
        public Response ExecuteFor<T>(T command, IBusinessRepository businessRepository, Guid? businessId) where T : ICommand
        {
            if (command.GetType() == typeof(BookingSetAttendanceCommand))
            {
                var useCase = new BookingSetAttendanceUseCase();
                useCase.Initialise(businessRepository, businessId);
                return useCase.SetAttendance(command as BookingSetAttendanceCommand);
            }

            if (command.GetType() == typeof(BookingSetPaymentStatusCommand))
            {
                var useCase = new BookingSetPaymentStatusUseCase();
                useCase.Initialise(businessRepository, businessId);
                return useCase.SetPaymentStatus(command as BookingSetPaymentStatusCommand);
            }

            throw new NotImplementedException();
        }
    }
}