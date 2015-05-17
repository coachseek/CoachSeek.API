using CoachSeek.Application.Services.Emailing;
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

        protected override void PostProcessing(CourseBooking newBooking)
        {
            var emailer = new OnlineBookingEmailer();
            emailer.Initialise(Context);

            var course = Context.BusinessRepository.GetCourse(BusinessId, newBooking.Course.Id);
            var coach = Context.BusinessRepository.GetCoach(BusinessId, course.Coach.Id);
            var customer = Context.BusinessRepository.GetCustomer(BusinessId, newBooking.Customer.Id);

            emailer.SendCourseEmailToCustomer(newBooking, course, coach, customer);
            emailer.SendCourseEmailToCoach(newBooking, course, coach, customer);
        }
    }
}