using System;
using Coachseek.DataAccess.Authentication.TableStorage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Coachseek.DataAccess.TableStorage.Authentication
{
    public class UserEntity : TableEntity
    {
        public UserEntity(string username)
        {
            PartitionKey = Constants.USER;
            RowKey = username;
        }

        public UserEntity() { }


        public Guid Id { get; set; }

        public Guid? BusinessId { get; set; }
        public string BusinessName { get; set; } // Debug

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        //public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
