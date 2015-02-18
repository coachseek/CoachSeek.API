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


        public SessionPresentation(PresentationCommand command, PresentationData servicePresentation)
        {
            command = BackfillMissingValuesFromService(command, servicePresentation);

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


        private PresentationCommand BackfillMissingValuesFromService(PresentationCommand sessionPresentation, PresentationData servicePresentation)
        {
            if (sessionPresentation == null)
            {
                var presentation = new PresentationCommand();

                if (servicePresentation != null && servicePresentation.Colour != null)
                {
                    presentation.Colour = servicePresentation.Colour;
                }

                return presentation;
            }

            if (SessionIsMissingColour(sessionPresentation) && ServiceHasColour(servicePresentation))
                sessionPresentation.Colour = servicePresentation.Colour;

            return sessionPresentation;
        }

        private bool SessionIsMissingColour(PresentationCommand sessionPresentation)
        {
            return sessionPresentation.Colour == null;
        }

        private bool ServiceHasColour(PresentationData servicePresentation)
        {
            return servicePresentation != null && servicePresentation.Colour != null;
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
