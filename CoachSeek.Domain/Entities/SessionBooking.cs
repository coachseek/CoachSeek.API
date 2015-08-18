using System.Collections.Generic;
using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class SessionBooking
    {
        private SessionStudentCapacity _studentCapacity;
        private IList<CustomerBookingData> Bookings { get; set; }

        public bool IsOnlineBookable { get; private set; }
        public int StudentCapacity { get { return _studentCapacity.Maximum; } }


        public SessionBooking(SessionBookingCommand command)
        {
            Bookings = new List<CustomerBookingData>();

            ValidateAndCreateSessionBooking(command);
        }

        public SessionBooking(SessionBookingData data)
        {
            CreateSessionBooking(data);
        }


        public SessionBookingData ToData()
        {
            var data = Mapper.Map<SessionBooking, SessionBookingData>(this);
            data.Bookings = Bookings;

            return data;
        }


        private void ValidateAndCreateSessionBooking(SessionBookingCommand command)
        {
            var errors = new ValidationException();

            ValidateAndCreateStudentCapacity(command.StudentCapacity, errors);

            errors.ThrowIfErrors();

            IsOnlineBookable = command.IsOnlineBookable;
        }

        private void CreateSessionBooking(SessionBookingData data)
        {
            _studentCapacity = new SessionStudentCapacity(data.StudentCapacity);
            IsOnlineBookable = data.IsOnlineBookable;
            Bookings = data.Bookings;
        }


        private void ValidateAndCreateStudentCapacity(int studentCapacity, ValidationException errors)
        {
            try
            {
                _studentCapacity = new SessionStudentCapacity(studentCapacity);
            }
            catch (StudentCapacityInvalid ex)
            {
                errors.Add(ex);
            }
        }
    }
}
