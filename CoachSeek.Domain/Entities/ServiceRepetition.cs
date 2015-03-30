using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class ServiceRepetition : Repetition
    {
        public ServiceRepetition(RepetitionData repetitionData)
        {
            CreateAndValidateRepetition(repetitionData);
        }

        public ServiceRepetition(RepetitionCommand repetitionCommand)
        {
            CreateAndValidateRepetition(repetitionCommand);
        }

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

        protected void CreateAndValidateRepetition(RepetitionData repetitionData)
        {
            var errors = new ValidationException();

            ValidateAndCreateSessionCount(repetitionData.SessionCount, errors);
            ValidateAndCreateRepeatFrequency(repetitionData.RepeatFrequency, errors);

            errors.ThrowIfErrors();

            if (IsSingleSession && HasRepeatFrequency)
                throw new ValidationException("For a single session the repeatFrequency must not be set.", RepeatFrequencyPath);
            if (IsRepeatingSession)
            {
                if (!HasRepeatFrequency)
                    throw new ValidationException("For a repeated session the repeatFrequency must be set.", RepeatFrequencyPath);
                if (_frequency.IsRepeatedEveryDay && SessionCount > MAXIMUM_DAILY_REPEAT)
                    throw new ValidationException(
                        string.Format("The maximum number of daily sessions is {0}.", MAXIMUM_DAILY_REPEAT), SessionCountPath);
                if (_frequency.IsRepeatedEveryWeek && SessionCount > MAXIMUM_WEEKLY_REPEAT)
                    throw new ValidationException(
                        string.Format("The maximum number of weekly sessions is {0}.", MAXIMUM_WEEKLY_REPEAT), SessionCountPath);
            }
        }
    }
}
