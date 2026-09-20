using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenter_Core.DTOs.Student;
using TrainingCenter_Core.DTOs.StudentProfile;

namespace TrainingCenter_Core.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync();
        Task<StudentResponseDto?> GetStudentByIdAsync(int StudentId);
        Task<StudentResponseDto> CreateStudentAsync(CreateStudentDto createStudentDto);
        Task<StudentResponseDto?> UpdateStudentAsync(int studentId, UpdateStudentDto updateStudentDto);
        Task<bool> DeleteStudentAsync(int studentId);
        Task<bool> UpdateStudentProfileAsync(int studentId, UpdateStudentProfileDto updateStudentProfileDto);
        Task<StudentResponseDto?> GetStudentByUserIdAsync(int UserId);

    }
}
