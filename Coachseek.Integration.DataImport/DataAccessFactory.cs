using CoachSeek.DataAccess.Main.Memory.Repositories;
using Coachseek.DataAccess.Main.SqlServer.Repositories;
using Coachseek.DataAccess.TableStorage.Logging;
using CoachSeek.Domain.Repositories;

namespace Coachseek.Integration.DataImport
{
    public class DataAccessFactory : IDataAccessFactory
    {
        private const string APPLICATION = "Data Import Processor";

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
            return new DataRepositories(new DbTestBusinessRepository(),
                                        new AzureTestTableLogRepository(APPLICATION));
        }

        private static DataRepositories CreateProductionRepositories()
        {
            return new DataRepositories(new DbBusinessRepository(),
                                        new AzureTableLogRepository(APPLICATION));
        }
    }
}
