using System.Threading.Tasks;
using System.Collections.Generic;
using ContosoUniversity.Core.Models;

namespace ContosoUniversity.Core.Repository
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAsync();
        Task<Student> GetAsync(int id);
        void Add(Student entity);
        void Update(Student entity);
        void Delete(int id);
    }
}
