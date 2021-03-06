﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omnichannel.Identity.Platform.Domain;

namespace Omnichannel.Identity.Platform.Infrastructure.Database.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users").HasIndex(u => u.ID);
            builder.Property(u => u.ID).HasColumnName("id");
            builder.Property(u => u.Company).HasColumnName("company").IsRequired();
            builder.Property(u => u.Name).HasColumnName("name").IsRequired();
            builder.Property(u => u.CPF).HasColumnName("cpf").IsRequired();
            builder.Property(u => u.Email).HasColumnName("email").IsRequired();
            builder.Property(u => u.Password).HasColumnName("password").IsRequired();
            builder.Property(u => u.Active).HasColumnName("active").IsRequired().HasDefaultValue(1);
            builder.Property(u => u.LogInToken).HasColumnName("login_token");
            builder.Property(u => u.LastLogin).HasColumnName("last_login");
            builder.Property(u => u.CreatedAt).HasColumnName("created_at").IsRequired().HasDefaultValueSql("now()");
            builder.Property(u => u.UpdatedAt).HasColumnName("updated_at");
        }
    }
}
