using System;
using System.Collections.Generic;

namespace ContosoUniversity.Core.Dto
{
    public class StudentDto : PersonDto
    {
        public DateTime EnrollmentDate { get; set; }

        public ICollection<EnrollmentDto> Enrollments { get; set; }
    }
}
