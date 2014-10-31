using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class ServiceDefaults
    {
        public int? Duration { get; private set; }
        public decimal? Price { get; set; }
        public int? StudentCapacity { get; set; }
        public bool? IsOnlineBookable { get; set; }
        public string Colour { get; set; }

        private Duration _duration { get; set; }
        private Price _price { get; set; }

        public ServiceDefaults(ServiceDefaultsData defaultsData)
        {
            var errors = new ValidationException();

            CreateDuration(defaultsData.Duration, errors);
            CreatePrice(defaultsData.Duration, errors);

            //_tuesday = CreateWorkingHours(workingHoursData.Tuesday, errors, "tuesday");
            //_wednesday = CreateWorkingHours(workingHoursData.Wednesday, errors, "wednesday");
            //_thursday = CreateWorkingHours(workingHoursData.Thursday, errors, "thursday");
            //_friday = CreateWorkingHours(workingHoursData.Friday, errors, "friday");
            //_saturday = CreateWorkingHours(workingHoursData.Saturday, errors, "saturday");
            //_sunday = CreateWorkingHours(workingHoursData.Sunday, errors, "sunday");

            errors.ThrowIfErrors();
        }


        private void CreateDuration(int? duration, ValidationException errors)
        {
            try
            {
                _duration = new Duration(duration);
            }
            catch (InvalidDuration)
            {
                errors.Add("The duration is not valid.", "service.duration");
            }
        }

        private void CreatePrice(decimal? price, ValidationException errors)
        {
            try
            {
                _price = new Price(price);
            }
            catch (InvalidPrice)
            {
                errors.Add("The price is not valid.", "service.price");
            }
        }
    }
}
