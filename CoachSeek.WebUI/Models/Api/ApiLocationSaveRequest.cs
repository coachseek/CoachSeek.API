using System;

namespace CoachSeek.WebUI.Models.Api
{
    public class ApiLocationSaveRequest
    {
        public Guid BusinessId { get; set; }
        public Guid? Id { get; set; }
        public string Name { get; set; }


        public bool IsExisting()
        {
            return Id.HasValue;
        }
    }
}