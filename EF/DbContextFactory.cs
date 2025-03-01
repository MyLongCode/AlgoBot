using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using AlgoBot.EF;
using Microsoft.Extensions.Options;

namespace AlgoBot.EF
{
    public class DbContextFactory : IDesignTimeDbContextFactory<BotDbContext>
    {
        private static string _connectionString = "Host=dpg-cueacvl2ng1s73862g00-a.frankfurt-postgres.render.com;Port=5432;Database=algobot_uc0i;Username=algobot;Password=Zq4pEkVfcgWfTesmDcNBAJqgr8Vn2fcS;";

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
