using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.UseCases
{
    public class CourseOnlineBookingAddUseCase : CourseBookingAddUseCase
    {
        public CourseOnlineBookingAddUseCase(RepeatedSession existingCourse)            
            : base(existingCourse)
        { }


        protected override void ValidateCommandAdditional(BookingAddCommand newBooking, ValidationException errors)
        {
            ValidateIsOnlineBookable(errors);
        }


        private void ValidateIsOnlineBookable(ValidationException errors)
        {
            if (!Course.Booking.IsOnlineBookable)
                errors.Add("This course is not online bookable.", "booking.session");
        }
    }
}