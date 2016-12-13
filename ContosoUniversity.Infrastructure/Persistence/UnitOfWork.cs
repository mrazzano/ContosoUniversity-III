using System.Threading.Tasks;
using ContosoUniversity.Core.Persistence;
using ContosoUniversity.Core.Repository;
using ContosoUniversity.Infrastructure.Database;
using ContosoUniversity.Infrastructure.Repository;

namespace ContosoUniversity.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SchoolContext _dbContext;

        public ICourseRepository Course { get; private set; }
        public IStudentRepository Student { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public IInstructorRepository Instructor { get; private set; }

        public UnitOfWork(SchoolContext dbContext)
        {
            _dbContext = dbContext;

            Course = new CourseRepository(dbContext);
            Student = new StudentRepository(dbContext);
            Department = new DepartmentRepository(dbContext);
            Instructor = new InstructorRepository(dbContext);
        }

        public async Task Complete()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
