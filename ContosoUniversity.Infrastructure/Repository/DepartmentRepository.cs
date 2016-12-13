using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;
using ContosoUniversity.Core.Models;
using ContosoUniversity.Core.Repository;
using ContosoUniversity.Infrastructure.Database;

namespace ContosoUniversity.Infrastructure.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly SchoolContext _dbContext;

        public DepartmentRepository(SchoolContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Department>> GetAsync()
        {
            return await _dbContext.Departments
                .Include(i => i.Administrator)
                .OrderBy(d=>d.Name)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<Department> GetAsync(int id)
        {
            return _dbContext.Departments
                .Include(i => i.Administrator)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.DepartmentId== id);
        }

        public void Add(Department entity)
        {
            _dbContext.Departments.Add(entity);
        }

        public void Update(Department entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete (int id)
        {
            var entity = _dbContext.Departments.Find(id);
            _dbContext.Departments.Remove(entity);
        }
    }
}
