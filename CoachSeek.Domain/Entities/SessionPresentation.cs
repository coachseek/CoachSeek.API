using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class SessionPresentation
    {
        public string Colour { get { return _colour.Colouration; } }

        private Colour _colour { get; set; }


        public SessionPresentation(PresentationData sessionPresentation, ServiceData service)
        {
            sessionPresentation = BackfillMissingValuesFromService(sessionPresentation, service);

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


        private PresentationData BackfillMissingValuesFromService(PresentationData sessionPresentation, ServiceData service)
        {
            if (sessionPresentation == null)
            {
                var presentation = new PresentationData();

                if (service.Defaults != null && service.Defaults.Colour != null)
                {
                    presentation.Colour = service.Defaults.Colour;
                }

                return presentation;
            }

            if (SessionIsMissingColour(sessionPresentation) && ServiceHasColour(service))
                sessionPresentation.Colour = service.Defaults.Colour;

            return sessionPresentation;
        }

        private bool SessionIsMissingColour(PresentationData sessionPresentation)
        {
            return sessionPresentation.Colour == null;
        }

        private bool ServiceHasColour(ServiceData service)
        {
            return service.Defaults != null && service.Defaults.Colour != null;
        }
    }
}
