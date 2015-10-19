using CoachSeek.DataAccess.Main.Memory.Repositories;
using Coachseek.DataAccess.Main.SqlServer.Repositories;
using Coachseek.DataAccess.TableStorage.Authentication;
using Coachseek.DataAccess.TableStorage.Emailing;
using Coachseek.DataAccess.TableStorage.Logging;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Api
{
    public static class DataAccessFactory
    {
        private const string APPLICATION = "API";


        public static DataRepositories CreateDataAccess(bool isTesting)
        {
            return isTesting ? CreateTestingRepositories() : CreateProductionRepositories();
        }


        private static DataRepositories CreateTestingRepositories()
        {
#if DEBUG
                                     // new InMemoryBusinessRepository()
                                     // new InMemoryUserRepository(), 
            return new DataRepositories(new DbTestBusinessRepository(),
                                        new AzureTestTableUserRepository(), 
                                        new AzureTestTableUnsubscribedEmailAddressRepository(),
                                        new HardCodedSupportedCurrencyRepository(),
                                        new AzureTestTableLogRepository(APPLICATION));
#else
            return new DataRepositories(new DbTestBusinessRepository(), 
                                        new AzureTestTableUserRepository(),
                                        new AzureTestTableUnsubscribedEmailAddressRepository(),
                                        new HardCodedSupportedCurrencyRepository(),
                                        new AzureTestTableLogRepository(APPLICATION));
#endif
        }

        private static DataRepositories CreateProductionRepositories()
        {
            return new DataRepositories(new DbBusinessRepository(), 
                                        new AzureTableUserRepository(),
                                        new AzureTableUnsubscribedEmailAddressRepository(),
                                        new HardCodedSupportedCurrencyRepository(),
                                        new AzureTableLogRepository(APPLICATION));
        }
    }
}