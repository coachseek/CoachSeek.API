using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.Services.Emailing;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.UseCases
{
    public class CourseSessionOnlineBookingAddUseCase : CourseSessionBookingAddUseCase, ICourseSessionOnlineBookingAddUseCase
    {
        private IEmailTemplateGetByTypeUseCase EmailTemplateGetByTypeUseCase { get; set; }

        public CourseSessionOnlineBookingAddUseCase(IEmailTemplateGetByTypeUseCase emailTemplateGetByTypeUseCase)
        {
            EmailTemplateGetByTypeUseCase = emailTemplateGetByTypeUseCase;
        }


        protected override void ValidateCommandAdditional(BookingAddCommand newBooking, ValidationException errors)
        {
            ValidateIsOnlineBookable(errors);
        }

        protected override CourseBooking CreateCourseBooking(BookingAddCommand command)
        {
            return new CourseOnlineBooking(command, Course);
        }

        private void ValidateIsOnlineBookable(ValidationException errors)
        {
            if (!Course.Booking.IsOnlineBookable)
                errors.Add(new CourseNotOnlineBookable(Course.Id));
        }

        protected override void PostProcessing(CourseBooking newBooking)
        {
            var emailer = new OnlineBookingEmailer(EmailTemplateGetByTypeUseCase);
            emailer.Initialise(Context);

            var coach = Context.BusinessContext.BusinessRepository.GetCoach(Business.Id, newBooking.Course.Coach.Id);
            var customer = Context.BusinessContext.BusinessRepository.GetCustomer(Business.Id, newBooking.Customer.Id);

            emailer.SendCourseEmailToCustomer(newBooking, coach, customer);
            emailer.SendCourseEmailToCoach(newBooking, coach, customer);
        }
    }
}
