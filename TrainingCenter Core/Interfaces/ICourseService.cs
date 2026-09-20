using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenter_Core.DTOs.Course;

namespace TrainingCenter_Core.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseResponseDto>> GetAllCoursesAsync();
        Task<CourseResponseDto?> GetCourseByIdAsync(int courseId);
        Task<CourseResponseDto> CreateCourseAsync(CreateCourseDto course);
        Task<CourseResponseDto?> UpdateCourseAsync(int courseId, UpdateCourseDto course);
        Task<bool> DeleteCourseAsync(int courseId);


    }
}
