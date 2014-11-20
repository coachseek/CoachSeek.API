using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class SessionPresentation
    {
        public string Colour { get { return _colour.Colouration; } }

        private Colour _colour { get; set; }


        public SessionPresentation(PresentationData sessionPresentation, PresentationData servicePresentation)
        {
            sessionPresentation = BackfillMissingValuesFromService(sessionPresentation, servicePresentation);

            try
            {
                _colour = new Colour(sessionPresentation.Colour);
            }
            catch (InvalidColour)
            {
                throw new ValidationException("The colour field is not valid.", "session.presentation.colour");
            }
        }


        public PresentationData ToData()
        {
            return Mapper.Map<SessionPresentation, PresentationData>(this);
        }


        private PresentationData BackfillMissingValuesFromService(PresentationData sessionPresentation, PresentationData servicePresentation)
        {
            if (sessionPresentation == null)
            {
                var presentation = new PresentationData();

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

        private bool SessionIsMissingColour(PresentationData sessionPresentation)
        {
            return sessionPresentation.Colour == null;
        }

        private bool ServiceHasColour(PresentationData servicePresentation)
        {
            return servicePresentation != null && servicePresentation.Colour != null;
        }
    }
}
