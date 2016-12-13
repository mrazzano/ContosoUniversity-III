using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;
using ContosoUniversity.Core.Models;
using ContosoUniversity.Core.Repository;
using ContosoUniversity.Infrastructure.Database;

namespace ContosoUniversity.Infrastructure.Repository
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly SchoolContext _dbContext;

        public InstructorRepository(SchoolContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Instructor>> GetAsync()
        {
            return await _dbContext.Instructors
                .Include(i => i.OfficeAssignment)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Instructor> GetAsync(int id)
        {
            return await _dbContext.Instructors
                .Where(i => i.Id == id)
                .Include(i => i.Courses)
                .Include(i => i.Courses.Select(c => c.Department))
                .Include(i => i.OfficeAssignment)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public void Add(Instructor entity)
        {
            foreach (var course in entity.Courses)
            {
                _dbContext.Entry(course).State = EntityState.Unchanged;
            }
            _dbContext.Instructors.Add(entity);
        }

        public void Update(Instructor entity)
        {
            var instructor = _dbContext.Instructors
                .Include(i => i.Courses)
                .Include(i => i.OfficeAssignment)
                .SingleOrDefault(i => i.Id == entity.Id);

            if (instructor == null)
                return;

            if (entity.OfficeAssignment != null)
            {
                instructor.OfficeAssignment = new OfficeAssignment()
                {
                    InstructorId = entity.Id,
                    Location = entity.OfficeAssignment.Location
                };
            }
            else
            {
                instructor.OfficeAssignment = null;
            }

            var courses = entity.Courses.Select(x => x.CourseId);
            UpdateInstructorCourses(instructor, courses);

            _dbContext.Entry(instructor).CurrentValues.SetValues(entity);
        }

        public void Delete(int id)
        {
            var instructor = _dbContext.Instructors.Find(id);
            if (instructor == null)
                return;

            if (instructor.OfficeAssignment != null)
            {
                instructor.OfficeAssignment = null;
            }

            var departments = _dbContext.Departments.Where(d => d.InstructorId == id).ToList();
            foreach (var department in departments)
            {
                department.InstructorId = null;
                _dbContext.Entry(department).State = EntityState.Modified;
            }
            _dbContext.Instructors.Remove(instructor);
        }

        public void UpdateInstructorCourses(Instructor entity, IEnumerable<int> courses)
        {
            var selectedCourses = new HashSet<int>(courses);
            var instructorCourses = new HashSet<int>(entity.Courses.Select(c => c.CourseId));

            foreach (var course in _dbContext.Courses.ToList())
            {
                if (selectedCourses.Contains(course.CourseId))
                {
                    if (!instructorCourses.Contains(course.CourseId))
                    {
                        entity.Courses.Add(course);
                    }
                }
                else
                {
                    if (instructorCourses.Contains(course.CourseId))
                    {
                        entity.Courses.Remove(course);
                    }
                }
            }
        }
    }
}
