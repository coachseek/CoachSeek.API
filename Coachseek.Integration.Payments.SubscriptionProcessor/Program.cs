using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.Contracts.UseCases.Payments;
using StructureMap;

namespace Coachseek.Integration.Payments.SubscriptionProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container(new TypeRegistry());
            var useCase = container.GetInstance<IProcessSubscriptionPaymentsUseCase>();
            useCase.ProcessAsync().Wait();
        }
    }
}
