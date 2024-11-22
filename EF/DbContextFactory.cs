using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using AlgoBot.EF;

namespace AlgoBot.EF
{
    public class DbContextFactory : IDesignTimeDbContextFactory<BotDbContext>
    {
        private static string _connectionString = "Host=dpg-cspfk5qj1k6c73as9960-a.oregon-postgres.render.com;Port=5432;Database=algobot_96j8;Username=danil;Password=PLY6paFqPjcKN8rxiSsnhvU29MP84XQO;";

        public BotDbContext CreateDbContext()
        {
            var builder = new DbContextOptionsBuilder<BotDbContext>();
            builder.UseNpgsql(_connectionString);

            return new BotDbContext(builder.Options);
        }

        public BotDbContext CreateDbContext(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
