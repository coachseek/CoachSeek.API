using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class ServicePresentation
    {
        public string Colour { get { return _colour.Colouration; } }

        private Colour _colour { get; set; }


        public ServicePresentation(PresentationData data)
        {
            try
            {
                if (data == null || data.Colour == null)
                    throw new ValidationException("The colour field is required.", "service.presentation.colour");

                _colour = new Colour(data.Colour);
            }
            catch (InvalidColour)
            {
                throw new ValidationException("The colour field is not valid.", "service.presentation.colour");
            }
        }

        public ServicePresentation(PresentationCommand command)
        {
            try
            {
                if (command == null || command.Colour == null)
                    throw new ValidationException("The colour field is required.", "service.presentation.colour");

                _colour = new Colour(command.Colour);
            }
            catch (InvalidColour)
            {
                throw new ValidationException("The colour field is not valid.", "service.presentation.colour");
            }
        }


        public PresentationData ToData()
        {
            return Mapper.Map<ServicePresentation, PresentationData>(this);
        }
    }
}
