using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class SessionBooking
    {
        private readonly StudentCapacity _studentCapacity;

        public bool IsOnlineBookable { get; private set; }

        public int? StudentCapacity { get { return _studentCapacity.Maximum; } }


        public SessionBooking(SessionBookingData data, ServiceData serviceData)
        {
            data = BackfillMissingValuesFromService(data, serviceData);
            Validate(data);

            _studentCapacity = new StudentCapacity(data.StudentCapacity);
            IsOnlineBookable = data.IsOnlineBookable.Value;
        }

        public SessionBooking(int? studentCapacity, bool isOnlineBookable)
        {
            _studentCapacity = new StudentCapacity(studentCapacity);
            IsOnlineBookable = isOnlineBookable;
        }


        private SessionBookingData BackfillMissingValuesFromService(SessionBookingData data, ServiceData serviceData)
        {
            if (data == null &&
                serviceData.Defaults != null &&
                serviceData.Defaults.StudentCapacity.HasValue &&
                serviceData.Defaults.IsOnlineBookable.HasValue)
            {
                return new SessionBookingData
                {
                    StudentCapacity = serviceData.Defaults.StudentCapacity.Value,
                    IsOnlineBookable = serviceData.Defaults.IsOnlineBookable.Value
                };
            }

            if (data.StudentCapacity == null && serviceData.Defaults != null)
                data.StudentCapacity = serviceData.Defaults.StudentCapacity;

            if (data.IsOnlineBookable == null && serviceData.Defaults != null)
                data.IsOnlineBookable = serviceData.Defaults.IsOnlineBookable;

            return data;
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

        public SessionBookingData ToData()
        {
            return Mapper.Map<SessionBooking, SessionBookingData>(this);
        }
    }
}
