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

        public ServicePresentation(PresentationData data)
        {
            _colour = new Colour(data.Colour);
        }


        public PresentationData ToData()
        {
            return Mapper.Map<ServicePresentation, PresentationData>(this);
        }
    }
}
