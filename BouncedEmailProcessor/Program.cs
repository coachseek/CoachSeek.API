using CoachSeek.Application.Contracts.UseCases.Emailing;
using StructureMap;

namespace Coachseek.Integration.Emailing.BouncedEmailProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container(new TypeRegistry());
            var useCase = container.GetInstance<IProcessBouncedEmailMessagesUseCase>();
            useCase.Process();
        }
    }
}
