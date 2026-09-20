using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenter_Core.DTOs.StudentProfile
{
    public record UpdateStudentProfileDto(string? Address, string? City,
        string? Country, string? Bio, string? LinkedInUrl);

}
