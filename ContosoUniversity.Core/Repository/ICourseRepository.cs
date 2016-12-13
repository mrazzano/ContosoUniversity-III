using System.Threading.Tasks;
using System.Collections.Generic;
using ContosoUniversity.Core.Models;

namespace ContosoUniversity.Core.Repository
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAsync();
        Task<Course> GetAsync(int id);
        void Add(Course entity);
        void Update(Course entity);
        void Delete(int id);
    }
}
