using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApiCharactersFilms.Constants;
using WebApiCharactersFilms.Repository.Entities;

namespace WebApiCharactersFilms.Repository.EntityConfigurations
{
    public class CharacterEntityConfiguration : IEntityTypeConfiguration<Character>
    {
        public void Configure(EntityTypeBuilder<Character> builder) {
            builder.HasKey(u => u.CharacterId);
            builder.Property(u => u.Name).IsRequired().HasMaxLength(Constraints.MaxLengthNames);
            builder.Property(u => u.PathFile).HasMaxLength(Constraints.MaxLengthPathFile);
            builder.Property(u => u.Biography).HasMaxLength(Constraints.MaxLengthBiography);
            
            builder.HasMany(a => a.CharacterFilms).WithOne(s=>s.Character).HasForeignKey(d=>d.CharacterId);
        }
    }
}
