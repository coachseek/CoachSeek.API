using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.Services.Emailing;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.UseCases
{
    public class StandaloneSessionOnlineBookingAddUseCase : StandaloneSessionBookingAddUseCase, IStandaloneSessionOnlineBookingAddUseCase
    {
        protected override void ValidateCommandAdditional(BookingAddCommand newBooking, 
                                                          ValidationException errors)
        {
            ValidateIsOnlineBookable(errors);
        }

        protected override SingleSessionBooking CreateSingleSessionBooking(SessionKeyCommand session,
                                                                           CustomerKeyCommand customer)
        {
            return new SingleSessionOnlineBooking(session, customer);
        }


        private void ValidateIsOnlineBookable(ValidationException errors)
        {
            if (!Session.Booking.IsOnlineBookable)
                errors.Add("A session is not online bookable.", "booking.sessions");
        }

        protected override void PostProcessing(SingleSessionBooking newBooking)
        {
            var emailer = new OnlineBookingEmailer();
            emailer.Initialise(Context);

            var session = Context.BusinessContext.BusinessRepository.GetSession(Business.Id, newBooking.Session.Id);
            var coach = Context.BusinessContext.BusinessRepository.GetCoach(Business.Id, session.Coach.Id);
            var customer = Context.BusinessContext.BusinessRepository.GetCustomer(Business.Id, newBooking.Customer.Id);

            emailer.SendSessionEmailToCustomer(newBooking, session, coach, customer);
            emailer.SendSessionEmailToCoach(newBooking, session, coach, customer);
        }
    }
}