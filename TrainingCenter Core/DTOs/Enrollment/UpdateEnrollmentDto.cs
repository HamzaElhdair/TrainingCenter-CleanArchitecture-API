using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenter_Core.DTOs.Enrollment
{
    public record UpdateEnrollmentDto(
       decimal ProgressPercent,
       decimal? FinalGrade,
       DateTime? CompletionDate,
       string Status);
}
