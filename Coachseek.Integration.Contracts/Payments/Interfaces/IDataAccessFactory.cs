using Coachseek.Integration.Contracts.Payments.Models;

namespace Coachseek.Integration.Contracts.Payments.Interfaces
{
    public interface IDataAccessFactory
    {
        DataRepositories CreateDataAccess(bool isTesting);
    }
}
