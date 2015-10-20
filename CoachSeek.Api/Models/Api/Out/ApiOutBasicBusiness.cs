using System;

namespace CoachSeek.Api.Models.Api.Out
{
    public class ApiOutBasicBusiness
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Sport { get; set; }
        public ApiOutBusinessPayment Payment { get; set; }

        public ApiOutBasicBusiness()
        {
            Payment = new ApiOutBusinessPayment();
        }
    }
}