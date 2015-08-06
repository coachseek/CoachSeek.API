using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.Contracts.UseCases.Executors;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Contracts;

namespace CoachSeek.Application.UseCases.Executors
{
    public class BookingUseCaseExecutor : IBookingUseCaseExecutor
    {
        public IBookingSetAttendanceUseCase BookingSetAttendanceUseCase { get; set; }
        public IBookingSetPaymentStatusUseCase BookingSetPaymentStatusUseCase { get; set; }

        public BookingUseCaseExecutor(IBookingSetAttendanceUseCase bookingSetAttendanceUseCase,
                                      IBookingSetPaymentStatusUseCase bookingSetPaymentStatusUseCase)
        {
            BookingSetAttendanceUseCase = bookingSetAttendanceUseCase;
            BookingSetPaymentStatusUseCase = bookingSetPaymentStatusUseCase;
        }


        public Response ExecuteFor<T>(T command, ApplicationContext context) where T : ICommand
        {
            if (command.GetType() == typeof(BookingSetAttendanceCommand))
            {
                BookingSetAttendanceUseCase.Initialise(context);
                return BookingSetAttendanceUseCase.SetAttendance(command as BookingSetAttendanceCommand);
            }

            if (command.GetType() == typeof(BookingSetPaymentStatusCommand))
            {
                BookingSetPaymentStatusUseCase.Initialise(context);
                return BookingSetPaymentStatusUseCase.SetPaymentStatus(command as BookingSetPaymentStatusCommand);
            }

            throw new NotImplementedException();
        }
    }
}