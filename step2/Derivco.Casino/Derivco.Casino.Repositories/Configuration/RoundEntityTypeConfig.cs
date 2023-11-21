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
    public class RoundEntityTypeConfig : IEntityTypeConfiguration<Round>
    {
        public void Configure(EntityTypeBuilder<Round> builder)
        {
            //builder.ToTable("Roud");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd(); ;

        }
    }
}
