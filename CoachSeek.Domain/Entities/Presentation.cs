using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public class Presentation
    {
        public string Colour { get { return _colour.Colouration; } }

        private Colour _colour { get; set; }

        public Presentation(PresentationData data)
        {
            _colour = new Colour(data.Colour);
        }

        public PresentationData ToData()
        {
            return Mapper.Map<Presentation, PresentationData>(this);
        }
    }
}
