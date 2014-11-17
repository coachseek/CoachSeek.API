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
            CreateSessionBooking(sessionBooking);
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

        private void CreateSessionBooking(SessionBookingData data)
        {
            var errors = new ValidationException();

            ValidateAndCreateStudentCapacity(data.StudentCapacity, errors);
            ValidateAndCreateIsOnlineBookable(data.IsOnlineBookable, errors);

            errors.ThrowIfErrors();
        }

        private void ValidateAndCreateStudentCapacity(int? studentCapacity, ValidationException errors)
        {
            if (!studentCapacity.HasValue)
            {
                errors.Add("The studentCapacity is required.", "session.booking.studentCapacity");
                return;
            }

            try
            {
                _studentCapacity = new SessionStudentCapacity(studentCapacity.Value);
            }
            catch (InvalidStudentCapacity)
            {
                errors.Add("The studentCapacity is not valid.", "session.booking.studentCapacity");
            }
        }

        private void ValidateAndCreateIsOnlineBookable(bool? isOnlineBookable, ValidationException errors)
        {
            if (!isOnlineBookable.HasValue)
                errors.Add("The isOnlineBookable is required.", "session.booking.isOnlineBookable");
            else
                IsOnlineBookable = isOnlineBookable.Value;
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
