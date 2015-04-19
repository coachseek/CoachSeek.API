using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Api.Models.Api.Out
{
    public abstract class ApiOutSession
    {
        public Guid Id { get; set; }

        public LocationKeyData Location { get; set; }
        public CoachKeyData Coach { get; set; }
        public ServiceKeyData Service { get; set; }

        public SessionTimingData Timing { get; set; }
        public PresentationData Presentation { get; set; }
    }
}