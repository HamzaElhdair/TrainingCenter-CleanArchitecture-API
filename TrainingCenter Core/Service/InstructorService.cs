using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenter_Core.DTOs.Instructor;
using TrainingCenter_Core.Entities;
using TrainingCenter_Core.Interfaces;

namespace TrainingCenter_Core.Service
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorRepository _instructorRepository;

        public InstructorService(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        private static InstructorResponseDto MapToResponseDto(Instructor instructor)
        {
            return new InstructorResponseDto
              (
               instructor.InstructorId,
               instructor.UserId,
               instructor.FirstName,
               instructor.LastName,
               instructor.Email,
               instructor.HireDate,
               instructor.Salary,
               instructor.IsActive,
               instructor.ManagerId

              );
        }


        public async Task<IEnumerable<InstructorResponseDto>> GetAllInstructorsAsync()
        {
            var instructor = await _instructorRepository.GetAllAsync();

            return instructor.Select(MapToResponseDto);

        }

        public async Task<InstructorResponseDto?> GetInstructorByIdAsync(int id)
        {
            var instructor = await _instructorRepository.GetByIdAsync(id);
            if (instructor == null) return null;

            return MapToResponseDto(instructor);
        }

        public async Task<InstructorResponseDto> CreateInstructorAsync(CreateInstructorDto createInstructorDto)
        {
            if (createInstructorDto.ManagerId.HasValue)
            {
                var managerExists = await _instructorRepository.GetByIdAsync(createInstructorDto.ManagerId.Value);
                if (managerExists == null)
                {
                    throw new ArgumentException($"Manager with ID {createInstructorDto.ManagerId} does not exist.");
                }
            }

            var instructor = new Instructor
            {
                FirstName = createInstructorDto.FirstName,
                LastName = createInstructorDto.LastName,
                Email = createInstructorDto.Email,
                HireDate = createInstructorDto.HireDate,
                Salary = createInstructorDto.Salary,
                IsActive = createInstructorDto.IsActive,
                ManagerId = createInstructorDto.ManagerId
            };
            await _instructorRepository.AddAsync(instructor);
            await _instructorRepository.SaveChangesAsync();

            return MapToResponseDto(instructor);
        }
        public async Task<InstructorResponseDto?> UpdateInstructorAsync(int InstructorId, UpdateInstructorDto updateInstructorDto)
        {
            if (updateInstructorDto.ManagerId.HasValue && updateInstructorDto.ManagerId.Value == InstructorId)
            {
                throw new ArgumentException("An instructor cannot be their own manager.");
            }

            var instructor = await _instructorRepository.GetByIdAsync(InstructorId);
            if (instructor == null) return null;

            if (updateInstructorDto.ManagerId.HasValue)
            {
                var managerExists = await _instructorRepository.GetByIdAsync(updateInstructorDto.ManagerId.Value);
                if (managerExists == null)
                {
                    throw new ArgumentException($"Manager with ID {updateInstructorDto.ManagerId} does not exist.");
                }
            }

            instructor.FirstName = updateInstructorDto.FirstName;
            instructor.LastName = updateInstructorDto.LastName;
            instructor.Email = updateInstructorDto.Email;
            instructor.HireDate = updateInstructorDto.HireDate;
            instructor.Salary = updateInstructorDto.Salary;
            instructor.IsActive = updateInstructorDto.IsActive;
            instructor.ManagerId = updateInstructorDto.ManagerId;

            _instructorRepository.Update(instructor);
            await _instructorRepository.SaveChangesAsync();

            return MapToResponseDto(instructor);
        }
        public async Task<bool> DeleteInstructorAsync(int InstructorId)
        {
            var instructor = await _instructorRepository.GetByIdAsync(InstructorId);
            if (instructor == null) return false;
            _instructorRepository.Delete(instructor);
            return await _instructorRepository.SaveChangesAsync();
        }

        public async Task<InstructorResponseDto?> GetInstructorByUserIdAsync(int UserId)
        {
            var instructor = await _instructorRepository.GetByIdWithUserId(UserId);
            if (instructor == null) return null;

            return MapToResponseDto(instructor);
        }
    }
}
