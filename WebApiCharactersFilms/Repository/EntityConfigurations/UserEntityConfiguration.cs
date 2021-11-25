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
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder) {
            builder.HasKey(u => u.UserId);
            builder.Property(u => u.Names).IsRequired().HasMaxLength(Constraints.MaxLengthNames);
            builder.Property(u => u.Surnames).IsRequired().HasMaxLength(Constraints.MaxLengthSurnames);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(Constraints.MaxLengthEmail);
            builder.Property(u => u.Password).IsRequired().HasMaxLength(Constraints.MaxLengthPasswordEncrypted);
            builder.Property(u => u.Role).IsRequired().HasMaxLength(Constraints.MaxLengthRole);
        }
    }
}
