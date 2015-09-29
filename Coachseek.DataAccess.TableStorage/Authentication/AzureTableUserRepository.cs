using System;
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

        public void Save(NewUser newUser)
        {
            var user = CreateUserEntity(newUser);
            Table.Execute(TableOperation.Insert(user));
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

        public void Save(User user)
        {
            try
            {
                Update(user);
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
            updateEntity.Id = user.Id;
            updateEntity.Email = user.Email;
            updateEntity.FirstName = user.FirstName;
            updateEntity.LastName = user.LastName;
            updateEntity.BusinessId = user.BusinessId;
            updateEntity.BusinessName = user.BusinessName;
        }

        public async Task<User> GetAsync(Guid id)
        {
            var query = new TableQuery<UserEntity>()
                            .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Constants.USER));

            return (from user in await Table.ExecuteQueryAsync(query) 
                    where user.Id == id 
                    select CreateUser(user))
                    .FirstOrDefault();
        }

        public User Get(Guid id)
        {
            var query = new TableQuery<UserEntity>()
                            .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Constants.USER));

            return (from user in Table.ExecuteQuery(query) 
                    where user.Id == id 
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

            //var query = new TableQuery<UserEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Constants.USER));
            var query = new TableQuery<UserEntity>().Where(TableQuery.GenerateFilterConditionForGuid("BusinessId", QueryComparisons.Equal, businessId));

            //var query = new TableQuery<UserEntity>().Where(TableQuery.CombineFilters(
            //                                               TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, Constants.USER),
            //                                               TableOperators.And,
            //                                               TableQuery.GenerateFilterCondition("BusinessId", QueryComparisons.Equal, stringBusinessId)));

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
                Id = newUser.Id,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                Phone = newUser.Phone,
                PasswordHash = newUser.PasswordHash,
                BusinessId = newUser.BusinessId,
                BusinessName = newUser.BusinessName,
            };
        }

        private User CreateUser(UserEntity user)
        {
            return new User(user.Id,
                            user.BusinessId,
                            user.BusinessName,
                            user.Email,
                            user.Phone,
                            user.FirstName,
                            user.LastName,
                            user.RowKey,
                            user.PasswordHash);
        }
    }
}
