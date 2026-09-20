using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenter_Core.DTOs.Enrollment
{
    public record EnrollmentResponseDto(int EnrollmentId,
                                     int StudentId,
                                     int CourseId,
                                     DateTime EnrollmentDate,
                                     DateTime? CompletionDate,
                                     decimal ProgressPercent,
                                     decimal? FinalGrade,
                                     string Status
                                   );
}
