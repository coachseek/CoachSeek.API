namespace Coachseek.Integration.Payments.PaymentsProcessor
{
    public interface IDataAccessFactory
    {
        DataRepositories CreateDataAccess(bool isTesting);
    }
}
