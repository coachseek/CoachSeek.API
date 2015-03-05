using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class SessionPresentation
    {
        public string Colour { get { return _colour.Colouration; } }

        private Colour _colour { get; set; }


        public SessionPresentation(PresentationCommand command)
        {
            ValidateAndCreateSessionPresentation(command);
        }

        public SessionPresentation(PresentationData data)
        {
            CreateSessionPresentation(data);
        }


        public PresentationData ToData()
        {
            return Mapper.Map<SessionPresentation, PresentationData>(this);
        }


        private void ValidateAndCreateSessionPresentation(PresentationCommand command)
        {
            try
            {
                _colour = new Colour(command.Colour);
            }
            catch (InvalidColour)
            {
                throw new ValidationException("The colour field is not valid.", "session.presentation.colour");
            }
        }

        private void CreateSessionPresentation(PresentationData data)
        {
            _colour = new Colour(data.Colour);
        }
    }
}
