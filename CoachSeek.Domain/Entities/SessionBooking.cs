using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class SessionBooking
    {
        private SessionStudentCapacity _studentCapacity;

        public bool IsOnlineBookable { get; private set; }

        public int StudentCapacity { get { return _studentCapacity.Maximum; } }


        public SessionBooking(SessionBookingData sessionBooking, ServiceBookingData serviceBooking)
        {
            sessionBooking = BackfillMissingValuesFromService(sessionBooking, serviceBooking);
            Validate(sessionBooking);

            CreateStudentCapacity(sessionBooking.StudentCapacity.Value);
            IsOnlineBookable = sessionBooking.IsOnlineBookable.Value;
        }

        public SessionBooking(int studentCapacity, bool isOnlineBookable)
        {
            CreateStudentCapacity(studentCapacity);
            IsOnlineBookable = isOnlineBookable;
        }

        public SessionBookingData ToData()
        {
            return Mapper.Map<SessionBooking, SessionBookingData>(this);
        }


        private SessionBookingData BackfillMissingValuesFromService(SessionBookingData sessionBooking, ServiceBookingData serviceBooking)
        {
            if (sessionBooking == null)
            {
                var booking = new SessionBookingData();

                if (serviceBooking != null)
                {
                    booking.StudentCapacity = serviceBooking.StudentCapacity;
                    booking.IsOnlineBookable = serviceBooking.IsOnlineBookable;
                }

                return booking;
            }

            if (sessionBooking.StudentCapacity == null && serviceBooking != null)
                sessionBooking.StudentCapacity = serviceBooking.StudentCapacity;

            if (sessionBooking.IsOnlineBookable == null && serviceBooking != null)
                sessionBooking.IsOnlineBookable = serviceBooking.IsOnlineBookable;

            return sessionBooking;
        }

        private void Validate(SessionBookingData data)
        {
            var errors = new ValidationException();

            if (data.StudentCapacity == null)
                errors.Add("The studentCapacity is not valid.", "session.booking.studentCapacity");

            if (data.IsOnlineBookable == null)
                errors.Add("The isOnlineBookable is not valid.", "session.booking.isOnlineBookable");

            errors.ThrowIfErrors();
        }

        private void CreateStudentCapacity(int studentCapacity)
        {
            try
            {
                _studentCapacity = new SessionStudentCapacity(studentCapacity);
            }
            catch (InvalidStudentCapacity)
            {
                throw new ValidationException("The studentCapacity is not valid.", "session.booking.studentCapacity");
            }
        }
    }
}
