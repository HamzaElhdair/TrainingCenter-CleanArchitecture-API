using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenter_Core.Entities;

namespace TrainingCenter_Infrastructure.Data.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.UserId).HasName("PK_Users");
            builder.HasIndex(e => e.Email, "IX_Users_Email").IsUnique();

            builder.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            builder.Property(e => e.Email).HasMaxLength(256);
            builder.Property(e => e.IsActive).HasDefaultValue(true);
            builder.Property(e => e.PasswordHash).HasMaxLength(256);
            builder.Property(e => e.Role).HasMaxLength(50);
            builder.Property(e => e.RefreshTokenHash).HasMaxLength((256));
            builder.Property(e => e.RefreshTokenExpiresAt);
            builder.Property(e => e.RefreshTokenRevokedAt);

        }
    }
}
