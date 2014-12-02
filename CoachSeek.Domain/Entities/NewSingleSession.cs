//using CoachSeek.Data.Model;
//using CoachSeek.Domain.Exceptions;

//namespace CoachSeek.Domain.Entities
//{
//    public class NewSingleSession : Session
//    {
//        public NewSingleSession(NewSessionData data, LocationData location, CoachData coach, ServiceData service)
//            : base(data, location, coach, service)
//        { }


//        public NewSingleSession(LocationData location,
//                       CoachData coach,
//                       ServiceData service,
//                       SessionTimingData timing,
//                       SessionBookingData booking,
//                       PricingData pricing,
//                       RepetitionData repetition,
//                       PresentationData presentation)
//        {
//            Id = id;

//            var errors = new ValidationException();

//            ValidateAndCreateLocation(location, errors);
//            ValidateAndCreateCoach(coach, errors);
//            ValidateAndCreateService(service, errors);
//            ValidateAndCreateSessionTiming(timing, service.Timing, errors);
//            ValidateAndCreateSessionBooking(booking, service.Booking, errors);
//            ValidateAndCreateSessionRepetition(repetition, service.Repetition, errors);
//            ValidateAndCreateSessionPricing(pricing, service.Pricing, errors);
//            ValidateAndCreateSessionPresentation(presentation, service.Presentation, errors);

//            errors.ThrowIfErrors();
//        }
//    }
//}
