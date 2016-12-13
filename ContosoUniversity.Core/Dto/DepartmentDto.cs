using System;

namespace ContosoUniversity.Core.Dto
{
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public DateTime StartDate { get; set; }
        public int? InstructorId { get; set; }
        public byte[] RowVersion { get; set; }
        public string AdministratorFirstMidName { get; set; }
        public string AdministratorLastName { get; set; }
    }
}
