using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCharactersFilms.Constants;
using WebApiCharactersFilms.Repository.Entities;

namespace WebApiCharactersFilms.Repository.EntityConfigurations
{
    public class GenreEntityConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder) {
            builder.HasKey(u => u.GenreId);
            builder.Property(u => u.PathFile).IsRequired().HasMaxLength(Constraints.MaxLengthPathFile);
            builder.Property(u => u.Name).IsRequired().HasMaxLength(Constraints.MaxLengthNames);
            builder.HasMany(u => u.Films).WithOne(a => a.Genre).HasForeignKey(s => s.GenreId);
        }
    }
}
