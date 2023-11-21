using Derivco.Casino.DomainModels.Roulette;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derivco.Casino.Repositories
{
    public class AppDBContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public AppDBContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sqlite database
            options.UseSqlite(Configuration.GetConnectionString("SQLLiteConnection"));
            //SQLitePCL.Batteries.Init();
        }

        public DbSet<Round> Rounds { get; set; }
        public DbSet<Bet> Bets { get; set; }       
        public DbSet<BetPayoutMap> PayoutMaps { get; set; }

    }
}
