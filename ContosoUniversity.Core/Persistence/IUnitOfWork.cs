using System.Threading.Tasks;
using ContosoUniversity.Core.Repository;

namespace ContosoUniversity.Core.Persistence
{
    public interface IUnitOfWork
    {
        Task Complete();
        ICourseRepository Course { get; }
        IStudentRepository Student { get; }
        IDepartmentRepository Department { get; }
        IInstructorRepository Instructor { get; }
    }
}