using CoachSeek.DataAccess.Main.Memory.Repositories;
using Coachseek.DataAccess.Main.SqlServer.Repositories;
using Coachseek.DataAccess.TableStorage.Logging;
using Coachseek.Integration.Contracts.Payments.Interfaces;

namespace Coachseek.Integration.Payments.PaymentsProcessor
{
    public class DataAccessFactory : IDataAccessFactory
    {
        private const string APPLICATION = "Payments Processor";

        public Contracts.Payments.Models.DataRepositories CreateDataAccess(bool isTesting)
        {
            return isTesting ? CreateTestingRepositories() : CreateProductionRepositories();
        }

        public Contracts.Payments.Models.DataRepositories CreateProductionDataAccess()
        {
            return CreateProductionRepositories();
        }


        private Contracts.Payments.Models.DataRepositories CreateTestingRepositories()
        {
            var dbTestBusinessRepository = new DbTestBusinessRepository();
            return new Contracts.Payments.Models.DataRepositories(dbTestBusinessRepository,
                                        dbTestBusinessRepository,
                                        new HardCodedSupportedCurrencyRepository(),
                                        new AzureTestTableLogRepository(APPLICATION));
        }

        private static Contracts.Payments.Models.DataRepositories CreateProductionRepositories()
        {
            var dbBusinessRepository = new DbBusinessRepository();
            return new Contracts.Payments.Models.DataRepositories(dbBusinessRepository,
                                        dbBusinessRepository,
                                        new HardCodedSupportedCurrencyRepository(),
                                        new AzureTableLogRepository(APPLICATION));
        }
    }
}
