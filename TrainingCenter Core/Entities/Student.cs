using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenter_Core.Entities
{
    public partial class Student
    {
        public int StudentId { get; set; }
        public int? UserId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }
        public DateTime RegisteredAt { get; set; }
        public string Status { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public virtual StudentProfile? StudentProfile { get; set; }
        public virtual User? User { get; set; } = null!;

    }
}
