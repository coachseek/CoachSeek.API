using CoachSeek.Domain.Repositories;
using Coachseek.Integration.Payments.PaymentsProcessor;
using DataRepositories = Coachseek.Integration.Payments.PaymentsProcessor.DataRepositories;

namespace Coachseek.Integration.Tests.Unit.Fakes
{
    public class StubDataAccessFactory : IDataAccessFactory
    {
        public bool WasCreateDataAccessCalled;
        public IBusinessRepository BusinessRepository;
        public ITransactionRepository TransactionRepository;

        public DataRepositories CreateDataAccess(bool isTesting)
        {
            WasCreateDataAccessCalled = true;

            return new DataRepositories(BusinessRepository, TransactionRepository);
        }
    }
}
