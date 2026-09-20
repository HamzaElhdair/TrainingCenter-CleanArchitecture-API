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
    public class EnrollmentConfigurations : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            // Indexes for faster search
            builder.HasIndex(e => e.CourseId, "IX_Enrollments_CourseId");
            builder.HasIndex(e => e.Status, "IX_Enrollments_Status");
            builder.HasIndex(e => e.StudentId, "IX_Enrollments_StudentId");


            // Prevent duplicate enrollment:
            // Same student cannot enroll twice
            builder.HasIndex(e => new { e.StudentId, e.CourseId },
                            "UQ_Enrollments_StudentId_CourseId")
                  .IsUnique();


            // Completion date
            builder.Property(e => e.CompletionDate)
                  .HasColumnType("datetime");


            // Default enrollment date = now
            builder.Property(e => e.EnrollmentDate)
                  .HasDefaultValueSql("(getdate())")
                  .HasColumnType("datetime");


            // Decimal grades
            builder.Property(e => e.FinalGrade)
                  .HasColumnType("decimal(5, 2)");


            builder.Property(e => e.ProgressPercent)
                  .HasColumnType("decimal(5, 2)");


            builder.Property(e => e.Status)
                  .HasMaxLength(20);


            // Many enrollments belong to one course
            builder.HasOne(d => d.Course)
                  .WithMany(p => p.Enrollments)
                  .HasForeignKey(d => d.CourseId)
                  .HasConstraintName("FK_Enrollments_Courses");


            // Many enrollments belong to one student
            builder.HasOne(d => d.Student)
                  .WithMany(p => p.Enrollments)
                  .HasForeignKey(d => d.StudentId)
                  .HasConstraintName("FK_Enrollments_Students");
        }
    }
}
