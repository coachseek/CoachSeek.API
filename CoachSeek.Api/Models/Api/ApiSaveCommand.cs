using System;

namespace CoachSeek.Api.Models.Api
{
    public class ApiSaveCommand : IApiIdable
    {
        public Guid? Id { get; set; }

        public bool IsNew() { return !Id.HasValue; }
    }
}