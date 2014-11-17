using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class ServiceBooking
    {
        private ServiceStudentCapacity _studentCapacity;

        public bool? IsOnlineBookable { get; private set; }

        public int? StudentCapacity { get { return _studentCapacity.Maximum; } }


        public ServiceBooking(ServiceBookingData data)
            : this(data.StudentCapacity, data.IsOnlineBookable)
        { }

        public ServiceBooking(int? studentCapacity, bool? isOnlineBookable)
        {
            CreateStudentCapacity(studentCapacity);
            IsOnlineBookable = isOnlineBookable;
        }

        public ServiceBookingData ToData()
        {
            return Mapper.Map<ServiceBooking, ServiceBookingData>(this);
        }


        private void CreateStudentCapacity(int? studentCapacity)
        {
            try
            {
                _studentCapacity = new ServiceStudentCapacity(studentCapacity);
            }
            catch (InvalidStudentCapacity)
            {
                throw new ValidationException("The studentCapacity is not valid.", "service.booking.studentCapacity");
            }
        }
    }
}
