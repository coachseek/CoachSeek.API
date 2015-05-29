using System;

namespace CoachSeek.Api.Models.Api.Out
{
    public abstract class ApiOutBusiness
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Currency { get; set; }
    }
}