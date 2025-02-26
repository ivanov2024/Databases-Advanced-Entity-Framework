using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicHub.Data.Models;

namespace MusicHub.Data.Configuration
{
    public class PerformerEntityConfiguration : IEntityTypeConfiguration<Performer>
    {
        public void Configure(EntityTypeBuilder<Performer> builder)
        {
            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.FirstName)
                .HasMaxLength(20)
                .IsRequired();

            builder
                .Property(p => p.LastName)
                .HasMaxLength(20)
                .IsRequired();

            builder
                .Property(p => p.Age)
                .IsRequired();

            builder
                .Property(p => p.NetWorth)
                .IsRequired();
        }
    }
}
