using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenter_Core.DTOs.User
{
    public record LoginUserDto(
  string Email,
  string Password
                           );
}
