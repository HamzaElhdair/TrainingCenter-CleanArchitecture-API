using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenter_Core.DTOs.Course
{
    public record UpdateCourseDto(string Title, string Code, string Description, decimal Price, 
        string Level, int DurationHours, DateTime? PublishedAt,
        string Status, int InstructorId);

}
