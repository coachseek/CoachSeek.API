using System.Collections.Generic;

namespace CoachSeek.Domain.Commands
{
    public class BookingAddCommand
    {
        public IList<SessionKeyCommand> Sessions { get; set; }
        public CustomerKeyCommand Customer { get; set; }

        public BookingAddCommand()
        {
            Sessions = new List<SessionKeyCommand>();
        }
    }
}