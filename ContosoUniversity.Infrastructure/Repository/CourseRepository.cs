using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;
using ContosoUniversity.Core.Models;
using ContosoUniversity.Core.Repository;
using ContosoUniversity.Infrastructure.Database;

namespace ContosoUniversity.Infrastructure.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly SchoolContext _dbContext;

        public CourseRepository(SchoolContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Course>> GetAsync()
        {
            return await _dbContext.Courses
                .Include(d => d.Department)
                .OrderBy(c => c.CourseId)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<Course> GetAsync(int id)
        {
            return _dbContext.Courses
                .Include(d => d.Department)
                .Include(d => d.Enrollments)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.CourseId == id);
        }

        public void Add(Course entity)
        {
            _dbContext.Entry(entity.Department).State = EntityState.Unchanged;
            _dbContext.Courses.Add(entity);
        }

        public void Update(Course entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var entity = _dbContext.Courses.Find(id);
            _dbContext.Courses.Remove(entity);
        }
    }
}
