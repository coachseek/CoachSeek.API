using CoachSeek.Application.Services.Emailing;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.UseCases
{
    public class SingleSessionOnlineBookingAddUseCase : SingleSessionBookingAddUseCase
    {
        public SingleSessionOnlineBookingAddUseCase(SingleSession existingSession)
            : base(existingSession)
        { }


        protected override void ValidateCommandAdditional(BookingAddCommand newBooking, ValidationException errors)
        {
            ValidateIsOnlineBookable(errors);
        }


        private void ValidateIsOnlineBookable(ValidationException errors)
        {
            if (!Session.Booking.IsOnlineBookable)
                errors.Add("This session is not online bookable.", "booking.session");
        }

        protected override void PostProcessing(SingleSessionBooking newBooking)
        {
            var emailer = new OnlineBookingEmailer();
            emailer.Initialise(Context);

            var session = Context.Business.BusinessRepository.GetSession(BusinessId, newBooking.Session.Id);
            var coach = Context.Business.BusinessRepository.GetCoach(BusinessId, session.Coach.Id);
            var customer = Context.Business.BusinessRepository.GetCustomer(BusinessId, newBooking.Customer.Id);

            emailer.SendSessionEmailToCustomer(newBooking, session, coach, customer);
            emailer.SendSessionEmailToCoach(newBooking, session, coach, customer);
        }
    }
}