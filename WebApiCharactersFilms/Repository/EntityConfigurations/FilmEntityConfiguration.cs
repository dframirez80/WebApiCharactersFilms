using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApiCharactersFilms.Constants;
using WebApiCharactersFilms.Repository.Entities;

namespace WebApiCharactersFilms.Repository.EntityConfigurations
{
    public class FilmEntityConfiguration : IEntityTypeConfiguration<Film>
    {
        public void Configure(EntityTypeBuilder<Film> builder) {
            builder.HasKey(u => u.FilmId);
            builder.Property(u => u.PathFile).HasMaxLength(Constraints.MaxLengthPathFile);
            builder.Property(u => u.Title).IsRequired().HasMaxLength(Constraints.MaxLengthTitle);

            builder.HasOne(a => a.Genre).WithMany(s => s.Films).HasForeignKey(a => a.GenreId);

            builder.HasMany(a => a.CharacterFilms).WithOne(s => s.Film).HasForeignKey(d=>d.FilmId);
        }
    }
}
