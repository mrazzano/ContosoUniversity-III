namespace ContosoUniversity.Core.Dto
{
    public class CourseDto
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}
