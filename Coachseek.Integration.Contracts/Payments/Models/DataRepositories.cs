using CoachSeek.Domain.Repositories;

namespace Coachseek.Integration.Contracts.Payments.Models
{
    public class DataRepositories
    {
        public IBusinessRepository BusinessRepository { get; private set; }
        public ITransactionRepository TransactionRepository { get; private set; }
        public ISupportedCurrencyRepository SupportedCurrencyRepository { get; private set; }
        public ILogRepository LogRepository { get; private set; }


        public DataRepositories(IBusinessRepository businessRepository,
                                ITransactionRepository transactionRepository,
                                ISupportedCurrencyRepository supportedCurrencyRepository,
                                ILogRepository logRepository)
        {
            BusinessRepository = businessRepository;
            TransactionRepository = transactionRepository;
            SupportedCurrencyRepository = supportedCurrencyRepository;
            LogRepository = logRepository;
        }
    }
}
