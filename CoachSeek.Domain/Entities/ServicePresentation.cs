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
            catch (ColourInvalid)
            {
                throw new ValidationException("The colour field is not valid.", "service.presentation.colour");
            }
        }

        public ServicePresentation(PresentationCommand command)
        {
            try
            {
                _colour = command == null ? new ColourDefault() : new Colour(command.Colour);
            }
            catch (ColourInvalid ex)
            {
                throw new ValidationException(ex);
            }
        }


        public PresentationData ToData()
        {
            return Mapper.Map<ServicePresentation, PresentationData>(this);
        }
    }
}
