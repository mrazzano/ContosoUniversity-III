using ContosoUniversity.Core.Models;

namespace ContosoUniversity.Core.Dto
{
    public class EnrollmentDto
    {
        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public Grade? Grade { get; set; }
        public CourseDto Course { get; set; }
    }
}
