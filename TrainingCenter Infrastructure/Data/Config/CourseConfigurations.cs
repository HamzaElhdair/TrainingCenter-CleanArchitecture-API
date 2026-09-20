using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrainingCenter_Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenter_Infrastructure.Data.Config
{
    public class CourseConfigurations : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            // Create index on InstructorId
            // Speeds searching courses by instructor
            builder.HasIndex(e => e.InstructorId, "IX_Courses_InstructorId");


            // Create index on Status
            builder.HasIndex(e => e.Status, "IX_Courses_Status");


            // Course Code must be unique
            // Example: C#101 cannot repeat
            builder.HasIndex(e => e.Code, "UQ_Courses_Code")
                  .IsUnique();


            // Maximum 30 characters
            builder.Property(e => e.Code)
                  .HasMaxLength(30);


            // Default value = current date/time
            builder.Property(e => e.CreatedAt)
                  .HasDefaultValueSql("(getdate())")
                  .HasColumnType("datetime");


            // Optional description max 500 chars
            builder.Property(e => e.Description)
                  .HasMaxLength(500);


            // Beginner / Intermediate / Advanced
            builder.Property(e => e.Level)
                  .HasMaxLength(30);


            // Decimal with precision (10,2)
            builder.Property(e => e.Price)
                  .HasColumnType("decimal(10, 2)");


            // Publish date
            builder.Property(e => e.PublishedAt)
                  .HasColumnType("datetime");


            // Active / Draft / Closed
            builder.Property(e => e.Status)
                  .HasMaxLength(20);


            // Course title max 150
            builder.Property(e => e.Title)
                  .HasMaxLength(150);


            // Relationship:
            // One Instructor has many Courses
            builder.HasOne(d => d.Instructor)
                  .WithMany(p => p.Courses)
                  .HasForeignKey(d => d.InstructorId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_Courses_Instructors");
        }



    }
}
