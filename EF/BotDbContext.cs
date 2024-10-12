using AlgoBot.EF.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Telegram.Bot.Types;

namespace Algo96.EF
{
    public class BotDbContext : DbContext
    {

        public virtual DbSet<BotUser> Users { get; set; } = null!;

        public BotDbContext(DbContextOptions<BotDbContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            Database.EnsureCreated();
        }
    }
}