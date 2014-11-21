using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public class ServiceRepetition : Repetition
    {
        public ServiceRepetition(RepetitionData repetitionData)
            : base(repetitionData)
        { }

        public override RepetitionData ToData()
        {
            return AutoMapper.Mapper.Map<ServiceRepetition, RepetitionData>(this);
        }

        protected override string SessionCountPath
        {
            get { return "service.repetition.sessionCount"; }
        }

        protected override string RepeatFrequencyPath
        {
            get { return "service.repetition.repeatFrequency"; }
        }
    }
}
