using AutoMapper;
using CoachSeek.Data.Model;
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
                _colour = new Colour(data.Colour);
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
