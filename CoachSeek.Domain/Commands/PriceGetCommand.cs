using System.Collections.Generic;

namespace CoachSeek.Domain.Commands
{
    public class PriceGetCommand
    {
        public IList<SessionKeyCommand> Sessions { get; set; }

        public PriceGetCommand()
        {
            Sessions = new List<SessionKeyCommand>();
        }
    }
}
