using System.Threading.Tasks;
using System.Collections.Generic;
using ContosoUniversity.Core.Models;

namespace ContosoUniversity.Core.Repository
{
    public interface IInstructorRepository
    {
        Task<IEnumerable<Instructor>> GetAsync();
        Task<Instructor> GetAsync(int id);
        void Add(Instructor entity);
        void Update(Instructor entity);
        void Delete(int id);
    }
}
