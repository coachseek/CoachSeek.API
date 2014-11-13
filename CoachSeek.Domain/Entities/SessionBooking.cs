using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public class SessionBooking
    {
        private readonly StudentCapacity _studentCapacity;

        public bool IsOnlineBookable { get; private set; }

        public int? StudentCapacity { get { return _studentCapacity.Maximum; } }


        public SessionBooking(SessionBookingData data)
            : this(data.StudentCapacity, data.IsOnlineBookable)
        { }

        public SessionBooking(int? studentCapacity, bool isOnlineBookable)
        {
            _studentCapacity = new StudentCapacity(studentCapacity);
            IsOnlineBookable = isOnlineBookable;
        }


        public SessionBookingData ToData()
        {
            return Mapper.Map<SessionBooking, SessionBookingData>(this);
        }
    }
}
