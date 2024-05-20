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
      
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
                         .SelectMany(t => t.GetProperties())
                         .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
        }

        public DbSet<Round> Rounds { get; set; }
        public DbSet<Bet> Bets { get; set; }       
        public DbSet<BetPayoutMap> PayoutMaps { get; set; }

    }
}
