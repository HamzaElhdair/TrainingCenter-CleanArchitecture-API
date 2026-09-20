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
    public class StudentConfigurations : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasIndex(e => e.Status, "IX_Students_Status");






            builder.Property(e => e.FirstName).HasMaxLength(50);
            builder.Property(e => e.LastName).HasMaxLength(50);
            builder.Property(e => e.PhoneNumber).HasMaxLength(30);


            // Registration date defaults to now
            builder.Property(e => e.RegisteredAt)
                  .HasDefaultValueSql("(getdate())")
                  .HasColumnType("datetime");


            builder.Property(e => e.Status).HasMaxLength(20);

            builder.HasOne(s => s.User)
            .WithOne(u => u.Student)
            .HasForeignKey<Student>(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
