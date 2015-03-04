namespace Coachseek.DataAccess.Main.SqlServer.Repositories
{
    public class DbTestBusinessRepository : DbBusinessRepository
    {
        protected override string ConnectionStringKey { get { return "BusinessDatabase-Test"; } }
    }
}
