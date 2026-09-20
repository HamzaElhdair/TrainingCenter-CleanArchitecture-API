using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenter_Core.DTOs.Student;
using TrainingCenter_Core.DTOs.StudentProfile;
using TrainingCenter_Core.Entities;
using TrainingCenter_Core.Interfaces;

namespace TrainingCenter_Core.Service
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        private static StudentResponseDto MapToResponseDto(Student student)
        {
            return new StudentResponseDto(
                  student.StudentId,
                  student.UserId,
                $"{student.FirstName} {student.LastName}",

                student.DateOfBirth,
                student.RegisteredAt,
                student.Status,
                student.PhoneNumber

            );
        }
        public async Task<IEnumerable<StudentResponseDto>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllAsync();


            return students.Select(MapToResponseDto);
        }
        public async Task<StudentResponseDto?> GetStudentByIdAsync(int studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student == null)
                return null;


            return MapToResponseDto(student);
        }
        public async Task<StudentResponseDto> CreateStudentAsync(CreateStudentDto createStudentDto)
        {
            var student = new Student
            {
                FirstName = createStudentDto.FirstName,
                LastName = createStudentDto.LastName,

                DateOfBirth = createStudentDto.DateOfBirth,
                RegisteredAt = DateTime.UtcNow,
                Status = createStudentDto.Status,
                PhoneNumber = createStudentDto.PhoneNumber
            };

            await _studentRepository.AddAsync(student);
            await _studentRepository.SaveChangesAsync();


            return MapToResponseDto(student);
        }
        public async Task<StudentResponseDto?> UpdateStudentAsync(int studentId, UpdateStudentDto updateStudentDto)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student == null)
                return null;

            student.FirstName = updateStudentDto.FirstName;
            student.LastName = updateStudentDto.LastName;

            student.DateOfBirth = updateStudentDto.DateOfBirth;
            student.Status = updateStudentDto.Status;
            student.PhoneNumber = updateStudentDto.PhoneNumber;
            _studentRepository.Update(student);
            await _studentRepository.SaveChangesAsync();


            return MapToResponseDto(student);
        }

        public async Task<bool> DeleteStudentAsync(int studentId)
        {
            var student = await _studentRepository.GetByIdAsync(studentId);
            if (student == null) return false;

            _studentRepository.Delete(student);

            return await _studentRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateStudentProfileAsync(int studentId, UpdateStudentProfileDto updateStudentProfileDto)
        {

            var student = await _studentRepository.GetByIdWithProfileAsync(studentId);
            if (student == null) return false;

            if (student.StudentProfile == null)
            {
                student.StudentProfile = new StudentProfile { StudentId = studentId };
            }

            student.StudentProfile.Address = updateStudentProfileDto.Address;
            student.StudentProfile.Bio = updateStudentProfileDto.Bio;
            student.StudentProfile.City = updateStudentProfileDto.City;
            student.StudentProfile.Country = updateStudentProfileDto.Country;
            student.StudentProfile.LinkedInUrl = updateStudentProfileDto.LinkedInUrl;

            return await _studentRepository.SaveChangesAsync();

        }

        public async Task<StudentResponseDto?> GetStudentByUserIdAsync(int UserId)
        {
            var student = await _studentRepository.GetByIdWithUserId(UserId);
            if (student == null)
                return null;


            return MapToResponseDto(student);
        }
    }
}
