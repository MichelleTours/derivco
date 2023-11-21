using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Derivco.Casino.DomainModels.Roulette;

namespace Derivco.Casino.Repositories.Configuration
{
    public class BetEntityTypeConfig : IEntityTypeConfiguration<Bet>
    {
        public void Configure(EntityTypeBuilder<Bet> builder)
        {

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd(); ;

            builder.HasOne(o => o.Round).WithMany().HasForeignKey(b => b.RoundId);

        }
    }
}
