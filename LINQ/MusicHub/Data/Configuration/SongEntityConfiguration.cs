using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicHub.Data.Models;

namespace MusicHub.Data.Configuration
{
    public class SongEntityConfiguration : IEntityTypeConfiguration<Song>
    {
        public void Configure(EntityTypeBuilder<Song> builder)
        {
            builder
                .HasKey(s => s.Id);

            builder
                .Property(s => s.Name)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(s => s.Duration)
                .IsRequired();

            builder
                .Property(s => s.CreatedOn)
                .IsRequired();

            builder
                .Property(s => s.Genre)
                .IsRequired();

            builder
                .HasOne(s => s.Album)
                .WithMany(a => a.Songs)
                .HasForeignKey(s => s.AlbumId)
                .IsRequired(false)  
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(s => s.Writer)
                .WithMany(w => w.Songs)
                .HasForeignKey(s => s.WriterId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(s => s.Price)
                .IsRequired();
        }
    }
}
