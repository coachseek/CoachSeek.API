using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class SessionRepetition : Repetition
    {
        public SessionRepetition(RepetitionData sessionRepetition, RepetitionData serviceRepetition)
        {
            sessionRepetition = BackfillMissingValuesFromService(sessionRepetition, serviceRepetition);
            CreateSessionRepetition(sessionRepetition);

        }

        public SessionRepetition(RepetitionData repetitionData)
            : base(repetitionData)
        { }


        public override RepetitionData ToData()
        {
            return AutoMapper.Mapper.Map<SessionRepetition, RepetitionData>(this);
        }


        protected override string RepeatTimesPath
        {
            get { return "session.repetition.repeatTimes"; }
        }

        protected override string RepeatFrequencyPath
        {
            get { return "session.repetition.repeatFrequency"; }
        }


        private void CreateSessionRepetition(RepetitionData sessionRepetition)
        {
            var errors = new ValidationException();

            CreateRepeatTimes(sessionRepetition.RepeatTimes, errors);
            CreateRepeatFrequency(sessionRepetition.RepeatFrequency, errors);

            errors.ThrowIfErrors();
        }

        private RepetitionData BackfillMissingValuesFromService(RepetitionData sessionRepetition, RepetitionData serviceRepetition)
        {
            return sessionRepetition ?? serviceRepetition;
        }
    }
}
