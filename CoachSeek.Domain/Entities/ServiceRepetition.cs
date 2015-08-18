using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Domain.Entities
{
    public class ServiceRepetition : Repetition
    {
        public ServiceRepetition(RepetitionCommand repetitionCommand)
        {
            CreateAndValidateRepetition(repetitionCommand);
        }

        public ServiceRepetition(RepetitionData repetitionData)
        {
            CreateRepetition(repetitionData);
        }

        public override RepetitionData ToData()
        {
            return AutoMapper.Mapper.Map<ServiceRepetition, RepetitionData>(this);
        }


        protected void CreateAndValidateRepetition(RepetitionData repetitionData)
        {
            var errors = new ValidationException();

            ValidateAndCreateSessionCount(repetitionData.SessionCount, errors);
            ValidateAndCreateRepeatFrequency(repetitionData.RepeatFrequency, errors);

            errors.ThrowIfErrors();

            if (IsSingleSession && HasRepeatFrequency)
                throw new ValidationException("For a single session the repeatFrequency must not be set.");
            if (IsCourse)
            {
                if (!HasRepeatFrequency)
                    throw new ValidationException("For a repeated session the repeatFrequency must be set.");
                if (_frequency.IsRepeatedEveryDay && SessionCount > MAXIMUM_DAILY_REPEAT)
                    throw new ValidationException(
                        string.Format("The maximum number of daily sessions is {0}.", MAXIMUM_DAILY_REPEAT));
                if (_frequency.IsRepeatedEveryWeek && SessionCount > MAXIMUM_WEEKLY_REPEAT)
                    throw new ValidationException(
                        string.Format("The maximum number of weekly sessions is {0}.", MAXIMUM_WEEKLY_REPEAT));
            }
        }
    }
}
