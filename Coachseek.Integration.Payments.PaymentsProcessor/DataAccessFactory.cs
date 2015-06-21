using System.Web.UI;
using CoachSeek.DataAccess.Main.Memory.Repositories;
using Coachseek.DataAccess.Main.SqlServer.Repositories;
using Coachseek.DataAccess.TableStorage.Logging;

namespace Coachseek.Integration.Payments.PaymentsProcessor
{
    public class DataAccessFactory : IDataAccessFactory
    {
        private const string APPLICATION = "Payments Processor";

        public DataRepositories CreateDataAccess(bool isTesting)
        {
            return isTesting ? CreateTestingRepositories() : CreateProductionRepositories();
        }

        public DataRepositories CreateProductionDataAccess()
        {
            return CreateProductionRepositories();
        }


        private DataRepositories CreateTestingRepositories()
        {
#if DEBUG
            return new DataRepositories(new DbTestBusinessRepository(),
                                        new InMemoryTransactionRepository(),
                                        //new AzureTestTableLogRepository(APPLICATION));
                                        new AzureTableLogRepository(APPLICATION));
#else
            return new DataRepositories(new DbTestBusinessRepository(), 
                                        new InMemoryTransactionRepository());
#endif
        }

        private static DataRepositories CreateProductionRepositories()
        {
            var dbBusinessRepository = new DbBusinessRepository();
            return new DataRepositories(dbBusinessRepository,
                                        dbBusinessRepository,
                                        new AzureTableLogRepository(APPLICATION));
        }
    }
}
