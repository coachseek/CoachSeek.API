using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

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
    }
}
