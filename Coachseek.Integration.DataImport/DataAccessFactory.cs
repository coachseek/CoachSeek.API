using Coachseek.DataAccess.Main.SqlServer.Repositories;
using Coachseek.DataAccess.TableStorage.Authentication;
using Coachseek.DataAccess.TableStorage.Emailing;
using Coachseek.DataAccess.TableStorage.Logging;

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
                                        new AzureTestTableLogRepository(APPLICATION),
                                        new AzureTestTableUserRepository(),
                                        new AzureTestTableUnsubscribedEmailAddressRepository());
        }

        private static DataRepositories CreateProductionRepositories()
        {
            return new DataRepositories(new DbBusinessRepository(),
                                        new AzureTableLogRepository(APPLICATION),
                                        new AzureTableUserRepository(),
                                        new AzureTableUnsubscribedEmailAddressRepository());
        }
    }
}
