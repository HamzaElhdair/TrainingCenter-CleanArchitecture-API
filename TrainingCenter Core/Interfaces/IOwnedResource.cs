using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenter_Core.Interfaces
{
    public interface IOwnedResource
    {
        int? UserId { get; }
    }
}
