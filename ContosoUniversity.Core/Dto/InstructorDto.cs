using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ContosoUniversity.Core.Dto
{
    public class InstructorDto : PersonDto
    {
        public DateTime HireDate { get; set; }

        public string OfficeAssignmentLocation { get; set; }

        public ICollection<CourseDto> Courses { get; private set; }

        public InstructorDto()
        {
            Courses = new Collection<CourseDto>();
        }
    }
}
