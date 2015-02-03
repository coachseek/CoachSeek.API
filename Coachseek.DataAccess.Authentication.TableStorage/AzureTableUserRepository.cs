﻿using System;
using System.Configuration;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Coachseek.DataAccess.Authentication.TableStorage
{
    public class AzureTableUserRepository : IUserRepository
    {
        private const string TABLE_NAME = "users";

        private CloudTableClient TableClient { get; set; }

        private CloudStorageAccount StorageAccount
        {
            get
            {
                var connectionString = ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString;
                return CloudStorageAccount.Parse(connectionString);
            }
        }

        private CloudTable UsersTable
        {
            get
            {
                TableClient = StorageAccount.CreateCloudTableClient();

                var usersTable = TableClient.GetTableReference(TABLE_NAME);
                usersTable.CreateIfNotExists();

                return usersTable;
            }
        }


        public User Save(NewUser newUser)
        {
            var user = new UserEntity(newUser.UserName)
            {
                Id = newUser.Id,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                PasswordHash = newUser.PasswordHash,
                BusinessId = newUser.BusinessId,
                BusinessName = newUser.BusinessName,
            };

            UsersTable.Execute(TableOperation.Insert(user));

            return newUser;
        }

        public User Save(User user)
        {
            try
            {
                return Update(user);
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode == 412)
                    return Update(user);

                throw;
            }
        }

        private User Update(User user)
        {
            var retrieveOperation = TableOperation.Retrieve<UserEntity>(Constants.USER, user.UserName);
            var retrievedResult = UsersTable.Execute(retrieveOperation);

            var updateEntity = (UserEntity)retrievedResult.Result;
            if (updateEntity == null)
                throw new Exception(); // TODO

            UpdateEntity(user, updateEntity);

            var updateOperation = TableOperation.Replace(updateEntity);

            UsersTable.Execute(updateOperation);

            return user;
        }

        private static void UpdateEntity(User user, UserEntity updateEntity)
        {
            updateEntity.Id = user.Id;
            updateEntity.Email = user.Email;
            updateEntity.FirstName = user.FirstName;
            updateEntity.LastName = user.LastName;
            updateEntity.BusinessId = user.BusinessId;
            updateEntity.BusinessName = user.BusinessName;
        }

        public User Get(Guid id)
        {
            var query = new TableQuery<UserEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Constants.USER));

            foreach (var user in UsersTable.ExecuteQuery(query))
            {
                if (user.Id == id)
                    return new User(user.Id, user.BusinessId, user.BusinessName, user.Email, user.FirstName, user.LastName, user.RowKey, user.PasswordHash);
            }

            return null;
        }

        public User GetByUsername(string username)
        {
            var retrieveOperation = TableOperation.Retrieve<UserEntity>(Constants.USER, username);

            var retrievedResult = UsersTable.Execute(retrieveOperation);
            if (retrievedResult.Result == null)
                return null;

            var user = (UserEntity) retrievedResult.Result;

            return new User(user.Id, user.BusinessId, user.BusinessName, user.Email, user.FirstName, user.LastName, user.RowKey, user.PasswordHash);
        }
    }
}
