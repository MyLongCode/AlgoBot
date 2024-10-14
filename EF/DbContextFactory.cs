using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Algo96.EF;

namespace AlgoBot.EF
{
    public class DbContextFactory : IDesignTimeDbContextFactory<BotDbContext>
    {
        private static string _connectionString;

        public BotDbContext CreateDbContext()
        {
            var builder = new DbContextOptionsBuilder<BotDbContext>();
            builder.UseNpgsql("W");

            return new BotDbContext(builder.Options);
        }

        public BotDbContext CreateDbContext(string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
