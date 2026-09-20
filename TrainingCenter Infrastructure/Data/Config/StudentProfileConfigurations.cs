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
    public class StudentProfileConfigurations : IEntityTypeConfiguration<StudentProfile>
    {
        public void Configure(EntityTypeBuilder<StudentProfile> builder)
        {
            // Primary key = StudentId
            builder.HasKey(e => e.StudentId);


            // Not auto increment
            builder.Property(e => e.StudentId)
                  .ValueGeneratedNever();


            builder.Property(e => e.Address).HasMaxLength(200);
            builder.Property(e => e.Bio).HasMaxLength(500);
            builder.Property(e => e.City).HasMaxLength(100);
            builder.Property(e => e.Country).HasMaxLength(100);
            builder.Property(e => e.LinkedInUrl).HasMaxLength(200);


            // One Student has one Profile
            builder.HasOne(d => d.Student)
                  .WithOne(p => p.StudentProfile)
                  .HasForeignKey<StudentProfile>(d => d.StudentId)
                  .HasConstraintName("FK_StudentProfiles_Students");
        }
    }
}
