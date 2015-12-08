using CoachSeek.DataAccess.Main.Memory.Repositories;
using CoachSeek.Domain.Repositories;
using Coachseek.Integration.Contracts.Payments.Interfaces;

namespace Coachseek.Integration.Tests.Unit.Payments.Fakes
{
    public class StubDataAccessFactory : IDataAccessFactory
    {
        public bool WasCreateDataAccessCalled;
        public bool WasCreateProductionDataAccessCalled;
        public IBusinessRepository BusinessRepository;
        public ITransactionRepository TransactionRepository;
        public ILogRepository LogRepository;


        public Contracts.Payments.Models.DataRepositories CreateDataAccess(bool isTesting)
        {
            WasCreateDataAccessCalled = true;

            return new Contracts.Payments.Models.DataRepositories(BusinessRepository, TransactionRepository, new HardCodedSupportedCurrencyRepository(), LogRepository);
        }

        public Contracts.Payments.Models.DataRepositories CreateProductionDataAccess()
        {
            WasCreateProductionDataAccessCalled = true;

            return new Contracts.Payments.Models.DataRepositories(BusinessRepository, TransactionRepository, new HardCodedSupportedCurrencyRepository(), LogRepository);
        }
    }
}
