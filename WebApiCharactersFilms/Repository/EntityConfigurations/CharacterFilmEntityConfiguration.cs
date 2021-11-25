using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Repository.Entities;

namespace WebApiCharactersFilms.Repository.EntityConfigurations
{
    public class CharacterFilmEntityConfiguration : IEntityTypeConfiguration<CharacterFilm>
    {
        public void Configure(EntityTypeBuilder<CharacterFilm> builder) {
            builder.HasKey(a => new { a.CharacterId, a.FilmId });
            builder.HasOne(a => a.Character).WithMany(s => s.CharacterFilms).HasForeignKey(d => d.CharacterId);
            builder.HasOne(a => a.Film).WithMany(s => s.CharacterFilms).HasForeignKey(d => d.FilmId);
        }
    }
}
