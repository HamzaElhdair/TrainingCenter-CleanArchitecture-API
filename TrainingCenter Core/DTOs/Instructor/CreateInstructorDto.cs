using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenter_Core.DTOs.Instructor
{
    public record CreateInstructorDto(string FirstName,
                                     string LastName,
                                     string Email,
                                     DateOnly HireDate,
                                     decimal Salary,
                                     bool IsActive,
                                     int? ManagerId
                                  );
}
