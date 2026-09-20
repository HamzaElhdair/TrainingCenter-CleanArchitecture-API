using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenter_Core.DTOs.Instructor;

namespace TrainingCenter_Core.Interfaces
{
    public interface IInstructorService
    {
        Task<IEnumerable<InstructorResponseDto>> GetAllInstructorsAsync();
        Task<InstructorResponseDto?> GetInstructorByIdAsync(int InstructorId);
        Task<InstructorResponseDto> CreateInstructorAsync(CreateInstructorDto createInstructorDto);
        Task<InstructorResponseDto?> UpdateInstructorAsync(int InstructorId, UpdateInstructorDto updateInstructorDto);
        Task<bool> DeleteInstructorAsync(int InstructorId);
        Task<InstructorResponseDto?> GetInstructorByUserIdAsync(int UserId);


    }
}
