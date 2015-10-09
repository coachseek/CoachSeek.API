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


        public string Id { get; set; } // Is really GUID

        public string BusinessId { get; set; } // Is really GUID
        public string BusinessName { get; set; } // Debug
        public string Role { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        //public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
