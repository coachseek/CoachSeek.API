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
            //var emailer = new OnlineBookingEmailer
            //{
            //    IsTesting = IsTesting,
            //    ForceEmail = ForceEmail,
            //    Sender = EmailSender
            //};

            //emailer.SendEmail(registration);
        }
    }
}