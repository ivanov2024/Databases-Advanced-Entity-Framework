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
    public class WriterEntityConfiguration : IEntityTypeConfiguration<Writer>
    {
        public void Configure(EntityTypeBuilder<Writer> builder)
        {
            builder
                .HasKey(w => w.Id);

            builder
                .Property(w => w.Name)
                .HasMaxLength(20)
                .IsRequired();

            builder
                .Property(w => w.Pseudonym)
                .IsRequired(false);
        }
    }
}
