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
            var dbTestBusinessRepository = new DbTestBusinessRepository();
            return new DataRepositories(dbTestBusinessRepository,
                                        dbTestBusinessRepository,
                                        new AzureTestTableLogRepository(APPLICATION));
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
