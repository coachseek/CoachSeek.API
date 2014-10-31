﻿namespace CoachSeek.Domain.Commands
{
    public class ServiceDefaultsCommand
    {
        public int? Duration { get; set; }
        public decimal? Price { get; set; }
        public int? StudentCapacity { get; set; }
        public bool? IsOnlineBookable { get; set; }
        public string Colour { get; set; }
    }
}
