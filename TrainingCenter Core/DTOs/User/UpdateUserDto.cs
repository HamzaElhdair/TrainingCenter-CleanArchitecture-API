using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenter_Core.DTOs.User
{
    public record UpdateUserDto(
    string Email,

    string Role,
    bool IsActive
);
}
