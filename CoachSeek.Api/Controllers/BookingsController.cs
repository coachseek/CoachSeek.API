using System.Net.Http;
using System.Web.Http;
using CoachSeek.Api.Attributes;
using CoachSeek.Api.Conversion;
using CoachSeek.Api.Filters;
using CoachSeek.Api.Models.Api.Booking;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.UseCases;

namespace CoachSeek.Api.Controllers
{
    public class BookingsController : BaseController
    {
        public IBookingAddUseCase BookingAddUseCase { get; set; }
        //public ICoachUpdateUseCase CoachUpdateUseCase { get; set; }

        public BookingsController()
        { }

        public BookingsController(IBookingAddUseCase bookingAddUseCase) //,
                                  //ICoachUpdateUseCase coachUpdateUseCase)
        {
            BookingAddUseCase = bookingAddUseCase;
            //CoachUpdateUseCase = coachUpdateUseCase;
        }


        // POST: api/Bookings
        [BasicAuthentication]
        [Authorize]
        [CheckModelForNull]
        [ValidateModelState]
        public HttpResponseMessage Post([FromBody]ApiBookingSaveCommand booking)
        {
            if (booking.IsNew())
                return AddBooking(booking);

            //return UpdateBooking(booking);
            return null;
        }

        // DELETE: api/Bookings/5
        public void Delete(int id)
        {
        }


        private HttpResponseMessage AddBooking(ApiBookingSaveCommand booking)
        {
            var command = BookingAddCommandConverter.Convert(booking);
            BookingAddUseCase.BusinessId = BusinessId;
            var response = BookingAddUseCase.AddBooking(command);
            return CreatePostWebResponse(response);
        }

        //private HttpResponseMessage UpdateBooking(ApiBookingSaveCommand booking)
        //{
        //    var command = CoachUpdateCommandConverter.Convert(BusinessId, booking);
        //    var response = CoachUpdateUseCase.UpdateCoach(command);
        //    return CreatePostWebResponse(response);
        //}
    }
}
