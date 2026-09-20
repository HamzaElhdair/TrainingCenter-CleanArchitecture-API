using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenter_Core.DTOs.Instructor
{
    public record UpdateInstructorDto(string FirstName, int UserId,
                                       string LastName,
                                       string Email,
                                       DateOnly HireDate,
                                       decimal Salary,
                                       bool IsActive,
                                       int? ManagerId
                                   );

}
