using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenter_Core.Interfaces;

namespace TrainingCenter_Core.DTOs.Student
{
    public record StudentResponseDto(int StudentId, int? UserId, string FullName,
        DateOnly DateOfBirth, DateTime RegisteredAt, string Status, string? PhoneNumber) : IOwnedResource;

}
