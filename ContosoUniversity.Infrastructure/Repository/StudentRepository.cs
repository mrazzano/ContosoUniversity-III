using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;
using ContosoUniversity.Core.Models;
using ContosoUniversity.Core.Repository;
using ContosoUniversity.Infrastructure.Database;

namespace ContosoUniversity.Infrastructure.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SchoolContext _dbContext;

        public StudentRepository(SchoolContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Student>> GetAsync()
        {
            return await _dbContext.Students
                .Include(e => e.Enrollments)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<Student> GetAsync(int id)
        {
            return _dbContext.Students
                .Include(e => e.Enrollments)
                .Include(e => e.Enrollments.Select(c => c.Course))
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public void Add(Student entity)
        {
            _dbContext.Students.Add(entity);
        }

        public void Update(Student entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var entity = _dbContext.Students.Find(id);
            _dbContext.Students.Remove(entity);
        }
    }
}
