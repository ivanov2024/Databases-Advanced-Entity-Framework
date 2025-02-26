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
    public class SongPerformerEntityConfiguration : IEntityTypeConfiguration<SongPerformer>
    {
        public void Configure(EntityTypeBuilder<SongPerformer> builder)
        {
            builder
                .HasKey(sp => new {sp.SongId, sp.PerformerId});

            builder
                .Property(sp => sp.SongId)
                .IsRequired();

            builder
                .Property(sp => sp.PerformerId)
                .IsRequired();

            builder
                .HasOne(sp => sp.Song)
                .WithMany(s => s.SongPerformers)
                .HasForeignKey(sp => sp.SongId);

            builder
                .HasOne(sp => sp.Performer)
                .WithMany(p => p.PerformerSongs)
                .HasForeignKey(sp => sp.PerformerId);
        }
    }
}
