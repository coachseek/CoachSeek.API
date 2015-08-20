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
            CreateSessionPresentation(command);
        }

        public SessionPresentation(PresentationData data)
        {
            CreateSessionPresentation(data);
        }


        public PresentationData ToData()
        {
            return Mapper.Map<SessionPresentation, PresentationData>(this);
        }


        private void CreateSessionPresentation(PresentationCommand command)
        {
            _colour = new Colour(command.Colour);
        }

        private void CreateSessionPresentation(PresentationData data)
        {
            _colour = new Colour(data.Colour);
        }
    }
}
