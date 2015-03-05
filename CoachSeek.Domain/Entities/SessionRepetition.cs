using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    public class SessionRepetition : Repetition
    {
        // Constructor for overriding service data with session data
        public SessionRepetition(RepetitionCommand sessionRepetition)
        {
            CreateAndValidateRepetition(sessionRepetition);
        }

        // Constructor for working with existing session repetitionData.
        public SessionRepetition(RepetitionData repetitionData)
        {
            CreateRepetition(repetitionData);
        }


        public override RepetitionData ToData()
        {
            return AutoMapper.Mapper.Map<SessionRepetition, RepetitionData>(this);
        }


        protected override string SessionCountPath
        {
            get { return "session.repetition.sessionCount"; }
        }

        protected override string RepeatFrequencyPath
        {
            get { return "session.repetition.repeatFrequency"; }
        }
    }
}
