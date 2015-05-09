﻿using System;
using Coachseek.DataAccess.Main.SqlServer.Repositories;
using Coachseek.DataAccess.TableStorage.Authentication;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.Api
{
    public static class DataAccessFactory
    {
        public static Tuple<IBusinessRepository, IUserRepository> CreateDataAccess(bool isTesting)
        {
            return isTesting ? CreateTestingRepositories() : CreateProductionRepositories();
        }


        private static Tuple<IBusinessRepository, IUserRepository> CreateTestingRepositories()
        {
#if DEBUG
                                                                // new InMemoryBusinessRepository()
            return new Tuple<IBusinessRepository, IUserRepository>(new DbTestBusinessRepository(), new AzureTestTableUserRepository());
#else
            return new Tuple<IBusinessRepository, IUserRepository>(new DbTestBusinessRepository(), new AzureTestTableUserRepository());
#endif
        }

        private static Tuple<IBusinessRepository, IUserRepository> CreateProductionRepositories()
        {
            return new Tuple<IBusinessRepository, IUserRepository>(new DbBusinessRepository(), new AzureTableUserRepository());
        }
    }
}