using CoachSeek.DataAccess.Main.Memory.Repositories;
using CoachSeek.Domain.Repositories;
using Coachseek.Integration.Payments.PaymentsProcessor;
using DataRepositories = Coachseek.Integration.Payments.PaymentsProcessor.DataRepositories;

namespace Coachseek.Integration.Tests.Unit.Payments.Fakes
{
    public class StubDataAccessFactory : IDataAccessFactory
    {
        public bool WasCreateDataAccessCalled;
        public bool WasCreateProductionDataAccessCalled;
        public IBusinessRepository BusinessRepository;
        public ITransactionRepository TransactionRepository;
        public ILogRepository LogRepository;


        public DataRepositories CreateDataAccess(bool isTesting)
        {
            WasCreateDataAccessCalled = true;

            return new DataRepositories(BusinessRepository, TransactionRepository, new HardCodedSupportedCurrencyRepository(), LogRepository);
        }

        public DataRepositories CreateProductionDataAccess()
        {
            WasCreateProductionDataAccessCalled = true;

            return new DataRepositories(BusinessRepository, TransactionRepository, new HardCodedSupportedCurrencyRepository(), LogRepository);
        }
    }
}
