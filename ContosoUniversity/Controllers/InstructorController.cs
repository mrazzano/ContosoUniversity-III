using System.Net;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using ContosoUniversity.Core.Models;
using ContosoUniversity.Core.ViewModels;
using ContosoUniversity.Service;

namespace ContosoUniversity.Controllers
{
    public class InstructorController : Controller
    {
        private readonly InstructorService _instructorService;
        private readonly CourseService _courseService;

        public InstructorController(InstructorService instructorService, CourseService courseService)
        {
            _instructorService = instructorService;
            _courseService = courseService;
        }

        // GET: Instructor
        public async Task<ActionResult> Index(int? id)
        {
            var viewModel = new InstructorDetailViewModel
            {
                Instructors = await _instructorService.Get()
            };

            if (!id.HasValue)
                return View(viewModel);

            ViewBag.InstructorId = id.Value;
            viewModel.Courses = viewModel.Instructors.Single(i => i.Id == id.Value).Courses;

            return View(viewModel);
        }

        // GET: Instructor/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var instructor = await _instructorService.Get(id.Value);
            if (instructor == null)
                return HttpNotFound();

            return View(instructor);
        }

        public async Task<ActionResult> Create()
        {
            var instructor = new Instructor();
            await PopulateAssignedCourseData(instructor);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "LastName,FirstMidName,HireDate,OfficeAssignment")]Instructor instructor, string[] selectedCourses)
        {
            if (ModelState.IsValid)
            {
                await UpdateInstructorCourses(instructor, selectedCourses);
                try
                {
                    await _instructorService.Add(instructor);
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            await PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        // GET: Instructor/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var instructor = await _instructorService.Get(id.Value);
            if (instructor == null)
                return HttpNotFound();

            await PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        // POST: Instructor/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int? id, string[] selectedCourses)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var instructor = await _instructorService.Get(id.Value);
            var fieldsToBind = new[] { "LastName", "FirstMidName", "HireDate", "OfficeAssignment" };
            if (TryUpdateModel(instructor, fieldsToBind))
            {
                await UpdateInstructorCourses(instructor, selectedCourses);
                try
                {
                    await _instructorService.Update(id.Value, instructor);
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            await PopulateAssignedCourseData(instructor);
            return View(instructor);
        }

        // GET: Instructor/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var instructor = await _instructorService.Get(id.Value);
            if (instructor == null)
                return HttpNotFound();

            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _instructorService.Delete(id);
                return RedirectToAction("Index");
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            var instructor = await _instructorService.Get(id);
            return View(instructor);
        }

        private async Task PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = await _courseService.Get();
            var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.CourseId));
            var viewModel = allCourses.Select(course => new AssignedCourseViewModel
            {
                CourseId = course.CourseId,
                Title = course.Title,
                Assigned = instructorCourses.Contains(course.CourseId)
            }).ToList();

            ViewBag.Courses = viewModel;
        }

        public async Task UpdateInstructorCourses(Instructor instructor, string[] courses)
        {
            if (instructor.Courses.Count > 0)
                instructor.Courses.Clear();

            if (courses == null)
                return;

            foreach (var selectedCourse in courses)
            {
                var course = await _courseService.Get(int.Parse(selectedCourse));
                instructor.Courses.Add(course);
            }
        }
    }
}
