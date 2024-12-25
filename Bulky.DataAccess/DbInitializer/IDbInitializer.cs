namespace Bulky.DataAccess.DbInitializer
{
    public interface IDbInitializer
    {
        void RunMigrations();
        void SeedRoles(string adminPassword);
    }
}
