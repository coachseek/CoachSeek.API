using CoachSeek.Data.Model;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class SessionRepetition : Repetition
    {
        public SessionRepetition(RepetitionData sessionRepetition, RepetitionData serviceRepetition)
        {
            sessionRepetition = BackfillMissingValuesFromService(sessionRepetition, serviceRepetition);
            //Validate(sessionRepetition);

            var errors = new ValidationException();

            CreateRepeatTimes(sessionRepetition.RepeatTimes, errors);
            CreateRepeatFrequency(sessionRepetition.RepeatFrequency, errors);

            errors.ThrowIfErrors();
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


        private RepetitionData BackfillMissingValuesFromService(RepetitionData sessionRepetition, RepetitionData serviceRepetition)
        {
            return sessionRepetition ?? serviceRepetition;
        }

        //private void Validate(RepetitionData repetition)
        //{
        //    var errors = new ValidationException();

        //    if (repetition.StudentCapacity == null)
        //        errors.Add("The studentCapacity is not valid.", "session.booking.studentCapacity");

        //    if (data.IsOnlineBookable == null)
        //        errors.Add("The isOnlineBookable is not valid.", "session.booking.isOnlineBookable");

        //    errors.ThrowIfErrors();
        //}
    }
}
