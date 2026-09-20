using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenter_Core.DTOs.Course;
using TrainingCenter_Core.Entities;
using TrainingCenter_Core.Interfaces;

namespace TrainingCenter_Core.Service
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        private static CourseResponseDto MapToResponseDto(Course course)
        {
            return new CourseResponseDto(
                course.CourseId, course.Title, course.Code,
                course.Description, course.Price, course.Level,
                course.DurationHours, course.CreatedAt, course.PublishedAt,
                course.Status, course.InstructorId);
        }

        public async Task<IEnumerable<CourseResponseDto>> GetAllCoursesAsync()
        {
            var course = await _courseRepository.GetAllAsync();

            return course.Select(MapToResponseDto);

        }
        public async Task<CourseResponseDto?> GetCourseByIdAsync(int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null)
                return null;

            return MapToResponseDto(course);

        }
        public async Task<CourseResponseDto> CreateCourseAsync(CreateCourseDto createCourseDto)
        {
            var course = new Course
            {
                Title = createCourseDto.Title,
                Code = createCourseDto.Code,
                Description = createCourseDto.Description,
                Price = createCourseDto.Price,
                Level = createCourseDto.Level,
                DurationHours = createCourseDto.DurationHours,
                CreatedAt = createCourseDto.CreatedAt,
                PublishedAt = createCourseDto.PublishedAt,
                Status = createCourseDto.Status,
                InstructorId = createCourseDto.InstructorId
            };
            await _courseRepository.AddAsync(course);
            await _courseRepository.SaveChangesAsync();

            return MapToResponseDto(course);
        }

        public async Task<CourseResponseDto?> UpdateCourseAsync(int courseId, UpdateCourseDto coursedto)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);

            course.Title = coursedto.Title;
            course.Code = coursedto.Code;
            course.Description = coursedto.Description;
            course.Price = coursedto.Price;
            course.Level = coursedto.Level;
            course.DurationHours = coursedto.DurationHours;
            course.PublishedAt = coursedto.PublishedAt;
            course.Status = coursedto.Status;
            course.InstructorId = coursedto.InstructorId;
            _courseRepository.Update(course);
            await _courseRepository.SaveChangesAsync();

            return MapToResponseDto(course);

        }

        public async Task<bool> DeleteCourseAsync(int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null) return false;
            _courseRepository.Delete(course);
            return await _courseRepository.SaveChangesAsync();

        }
    }
}
