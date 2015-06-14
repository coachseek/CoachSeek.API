using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.DataAccess.Main.Memory.Configuration;
using StructureMap;

namespace Coachseek.Integration.Payments.PaymentsProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            DbAutoMapperConfigurator.Configure();

            var container = new Container(new TypeRegistry());
            var useCase = container.GetInstance<IProcessPaymentsUseCase>();

            //while (true)
                useCase.Process();
        }
    }
}
