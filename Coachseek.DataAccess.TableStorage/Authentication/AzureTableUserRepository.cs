﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coachseek.DataAccess.Authentication.TableStorage;
using Coachseek.DataAccess.TableStorage.Extensions;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Coachseek.DataAccess.TableStorage.Authentication
{
    public class AzureTableUserRepository : AzureTableRepositoryBase, IUserRepository
    {
        protected override string TableName { get { return "users"; } }


        public async Task SaveAsync(NewUser newUser)
        {
            var user = CreateUserEntity(newUser);
            await Table.ExecuteAsync(TableOperation.Insert(user));
        }

        public async Task SaveAsync(User user)
        {
            try
            {
                await UpdateAsync(user);
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode == 412)
                    Update(user);

                throw;
            }
        }

        private async Task UpdateAsync(User user)
        {
            var retrieveOperation = TableOperation.Retrieve<UserEntity>(Constants.USER, user.Username);
            var retrievedResult = await Table.ExecuteAsync(retrieveOperation);

            var updateEntity = (UserEntity)retrievedResult.Result;
            if (updateEntity == null)
                throw new Exception(); // TODO

            UpdateEntity(user, updateEntity);
            var updateOperation = TableOperation.Replace(updateEntity);
            await Table.ExecuteAsync(updateOperation);
        }

        private void Update(User user)
        {
            var retrieveOperation = TableOperation.Retrieve<UserEntity>(Constants.USER, user.Username);
            var retrievedResult = Table.Execute(retrieveOperation);

            var updateEntity = (UserEntity)retrievedResult.Result;
            if (updateEntity == null)
                throw new Exception(); // TODO

            UpdateEntity(user, updateEntity);
            var updateOperation = TableOperation.Replace(updateEntity);
            Table.Execute(updateOperation);
        }

        private static void UpdateEntity(User user, UserEntity updateEntity)
        {
            updateEntity.Id = user.Id.ToString().ToLowerInvariant();
            updateEntity.Email = user.Email;
            updateEntity.FirstName = user.FirstName;
            updateEntity.LastName = user.LastName;
            updateEntity.BusinessId = user.BusinessId.ToString().ToLowerInvariant();
            updateEntity.BusinessName = user.BusinessName;
            updateEntity.Role = user.Role;
        }

        private async Task<IList<UserEntity>> GetAllUserEntitiesAsync()
        {
            return await Table.ExecuteQueryAsync(GetAllUsersQuery());
        }

        private TableQuery<UserEntity> GetAllUsersQuery()
        {
            return new TableQuery<UserEntity>()
                            .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Constants.USER));
        }

        public async Task<IList<User>> GetAllAsync()
        {
            return (from user in await GetAllUserEntitiesAsync()
                    select CreateUser(user))
                    .ToList();
        }

        public async Task<User> GetAsync(Guid id)
        {
            return (from user in await GetAllUserEntitiesAsync() 
                    where user.Id == id.ToString().ToLowerInvariant()
                    select CreateUser(user))
                    .FirstOrDefault();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var retrieveOperation = TableOperation.Retrieve<UserEntity>(Constants.USER, username);

            var retrievedResult = await Table.ExecuteAsync(retrieveOperation);
            if (retrievedResult.Result == null)
                return null;

            var user = (UserEntity) retrievedResult.Result;

            return CreateUser(user);
        }

        public User GetByUsername(string username)
        {
            var retrieveOperation = TableOperation.Retrieve<UserEntity>(Constants.USER, username);

            var retrievedResult = Table.Execute(retrieveOperation);
            if (retrievedResult.Result == null)
                return null;

            var user = (UserEntity)retrievedResult.Result;

            return CreateUser(user);
        }

        public User GetByBusinessId(Guid businessId)
        {
            var stringBusinessId = businessId.ToString().ToLower();
            var query = new TableQuery<UserEntity>().Where(TableQuery.GenerateFilterCondition("BusinessId", QueryComparisons.Equal, stringBusinessId));
            var results = Table.ExecuteQuery(query).ToList();
            if (!results.Any())
                return null;
            var user = results.First();
            return CreateUser(user);
        }


        private UserEntity CreateUserEntity(NewUser newUser)
        {
            return new UserEntity(newUser.Username)
            {
                Id = newUser.Id.ToString().ToLowerInvariant(),
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                Phone = newUser.Phone,
                PasswordHash = newUser.PasswordHash,
                BusinessId = newUser.BusinessId.ToString().ToLowerInvariant(),
                BusinessName = newUser.BusinessName,
                Role = newUser.Role
            };
        }

        private User CreateUser(UserEntity user)
        {
            return new User(new Guid(user.Id),
                            GetBusinessId(user.BusinessId),
                            user.BusinessName,
                            user.Role,
                            user.Email,
                            user.Phone,
                            user.FirstName,
                            user.LastName,
                            user.RowKey,
                            user.PasswordHash);
        }

        private Guid? GetBusinessId(string businessId)
        {
            if (string.IsNullOrEmpty(businessId))
                return null;
            return new Guid(businessId);
        }
    }
}
