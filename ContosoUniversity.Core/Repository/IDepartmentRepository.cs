using System.Threading.Tasks;
using System.Collections.Generic;
using ContosoUniversity.Core.Models;

namespace ContosoUniversity.Core.Repository
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAsync();
        Task<Department> GetAsync(int id);
        void Add(Department entity);
        void Update(Department entity);
        void Delete(int id);
    }
}
