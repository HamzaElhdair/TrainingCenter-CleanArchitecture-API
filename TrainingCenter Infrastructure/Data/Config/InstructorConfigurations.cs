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
    public class InstructorConfigurations : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            // Self-reference manager relationship
            builder.HasIndex(e => e.ManagerId, "IX_Instructors_ManagerId");


            // Email must be unique
            builder.HasIndex(e => e.Email, "UQ_Instructors_Email")
                  .IsUnique();


            builder.Property(e => e.Email).HasMaxLength(150);
            builder.Property(e => e.FirstName).HasMaxLength(50);


            // Default = active instructor
            builder.Property(e => e.IsActive)
                  .HasDefaultValue(true);


            builder.Property(e => e.LastName).HasMaxLength(50);


            builder.Property(e => e.Salary)
                  .HasColumnType("decimal(10, 2)");


            // One manager can manage many instructors
            builder.HasOne(d => d.Manager)
                  .WithMany(p => p.InverseManager)
                  .HasForeignKey(d => d.ManagerId)
                  .HasConstraintName("FK_Instructors_Manager");

            builder.HasOne(s => s.User)
            .WithOne(u => u.Instructor)
            .HasForeignKey<Instructor>(s => s.UserId);

        }
    }
}
