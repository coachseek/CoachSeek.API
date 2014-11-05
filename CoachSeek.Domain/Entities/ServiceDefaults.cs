using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class ServiceDefaults
    {
        public int? Duration { get { return _duration.Minutes; } }
        public int? StudentCapacity { get { return _capacity.Maximum; } }
        public bool? IsOnlineBookable { get; private set; }
        public string Colour { get { return _colour.Colouration; } }

        private Duration _duration { get; set; }
        private StudentCapacity _capacity { get; set; }
        private Colour _colour { get; set; }


        public ServiceDefaults(ServiceDefaultsData defaultsData)
        {
            var errors = new ValidationException();

            CreateDuration(defaultsData.Duration, errors);
            CreateStudentCapacity(defaultsData.StudentCapacity, errors);
            CreateColour(defaultsData.Colour, errors);
            IsOnlineBookable = defaultsData.IsOnlineBookable;

            errors.ThrowIfErrors();
        }


        public ServiceDefaultsData ToData()
        {
            return AutoMapper.Mapper.Map<ServiceDefaults, ServiceDefaultsData>(this);
        }

        private void CreateDuration(int? duration, ValidationException errors)
        {
            try
            {
                _duration = new Duration(duration);
            }
            catch (InvalidDuration)
            {
                errors.Add("The duration is not valid.", "service.defaults.duration");
            }
        }

        private void CreateStudentCapacity(int? maximum, ValidationException errors)
        {
            try
            {
                _capacity = new StudentCapacity(maximum);
            }
            catch (InvalidStudentCapacity)
            {
                errors.Add("The studentCapacity is not valid.", "service.defaults.studentCapacity");
            }
        }

        private void CreateColour(string colour, ValidationException errors)
        {
            try
            {
                _colour = new Colour(colour);
            }
            catch (InvalidColour)
            {
                errors.Add("The colour is not valid.", "service.defaults.colour");
            }
        }
    }
}
