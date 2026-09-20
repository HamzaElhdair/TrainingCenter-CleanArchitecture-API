using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenter_Core.DTOs.Course
{
    public record CreateCourseDto(string Title, string Code, string Description, 
        decimal Price, string Level, int DurationHours, 
        DateTime CreatedAt, DateTime? PublishedAt, 
        string Status, int InstructorId);

}
