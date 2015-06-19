using System;
using CoachSeek.DataAccess.Main.Memory.Repositories;
using Coachseek.DataAccess.Main.SqlServer.Repositories;

namespace Coachseek.Integration.Payments.PaymentsProcessor
{
    public class DataAccessFactory : IDataAccessFactory
    {
        public DataRepositories CreateDataAccess(bool isTesting)
        {
            return isTesting ? CreateTestingRepositories() : CreateProductionRepositories();
        }

        private DataRepositories CreateTestingRepositories()
        {
#if DEBUG
            return new DataRepositories(new DbTestBusinessRepository(),
                                        new InMemoryTransactionRepository());
#else
            return new DataRepositories(new DbTestBusinessRepository(), 
                                        new InMemoryTransactionRepository());
#endif
        }

        private static DataRepositories CreateProductionRepositories()
        {
            throw new InvalidOperationException(); 

            //return new DataRepositories(new DbBusinessRepository(),
            //                            new InMemoryTransactionRepository());
        }
    }
}
