using CoachSeek.Domain.Repositories;

namespace Coachseek.Integration.Payments.PaymentsProcessor
{
    public class DataRepositories
    {
        public IBusinessRepository BusinessRepository { get; private set; }
        public ITransactionRepository TransactionRepository { get; private set; }


        public DataRepositories(IBusinessRepository businessRepository,
                                ITransactionRepository transactionRepository)
        {
            BusinessRepository = businessRepository;
            TransactionRepository = transactionRepository;
        }
    }
}
