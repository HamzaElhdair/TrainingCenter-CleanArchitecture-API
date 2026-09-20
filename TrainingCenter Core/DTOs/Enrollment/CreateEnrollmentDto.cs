using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenter_Core.DTOs.Enrollment
{
    public record CreateEnrollmentDto(int StudentId,
                                       int CourseId,
                                       DateTime? CompletionDate,
                                       decimal ProgressPercent = 0,
                                       string Status = "Active");
}
