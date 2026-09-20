using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenter_Core.Interfaces;

namespace TrainingCenter_Core.DTOs.Instructor
{
    public record InstructorResponseDto(int InstructorId,
                                       int? UserId,
                                        string FirstName,
                                       string LastName,
                                        string Email,
                                        DateOnly HireDate,
                                        decimal Salary,
                                        bool IsActive,
                                        int? ManagerId) : IOwnedResource;
}

