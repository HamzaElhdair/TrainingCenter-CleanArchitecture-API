using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenter_Core.DTOs.Student
{
    public record UpdateStudentDto(int UserId, string FirstName, string LastName, 
        string Email, DateOnly DateOfBirth, string Status, string? PhoneNumber);

}
