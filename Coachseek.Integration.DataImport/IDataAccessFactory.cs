using CoachSeek.Domain.Repositories;

namespace Coachseek.Integration.DataImport
{
    public interface IDataAccessFactory
    {
        DataRepositories CreateDataAccess(bool isTesting);
    }
}
