using System;
using System.Collections.Generic;
using System.Linq;

namespace CoachSeek.Domain.Commands
{
    public class BookingAddCommand
    {
        public IList<SessionKeyCommand> Sessions { get; set; }
        public CustomerKeyCommand Customer { get; set; }
        public bool IsOnlineBooking { get; set; }
        public string DiscountCode { get; set; }

        public BookingAddCommand()
        {
            Sessions = new List<SessionKeyCommand>();
        }

        public bool Contains(Guid sessionId)
        {
            return Sessions.Select(x => x.Id).Contains(sessionId);
        }
    }
}