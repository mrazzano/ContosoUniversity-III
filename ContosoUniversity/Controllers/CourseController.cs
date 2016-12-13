using System.Net;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using ContosoUniversity.Core.Models;
using ContosoUniversity.Service;

namespace ContosoUniversity.Controllers
{
    public class CourseController : Controller
    {
        private readonly CourseService _courseService;
        private readonly DepartmentService _departmentService;

        public CourseController(CourseService courseService, DepartmentService departmentService)
        {
            _courseService = courseService;
            _departmentService = departmentService;
        }

        // GET: Course
        public async Task<ActionResult> Index(int? departmentId)
        {
            await PopulateDepartmentDropDown(departmentId);

            var courses = await _courseService.Get();
            if (departmentId.HasValue)
            {
                courses = courses.Where(c => c.DepartmentId == departmentId.Value);
            }

            return View(courses);
        }

        // GET: Course/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var course = await _courseService.Get(id.Value);
            if (course == null)
                return HttpNotFound();

            return View(course);
        }

        public async Task<ActionResult> Create()
        {
            await PopulateDepartmentDropDown(null);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CourseId,Title,Credits,DepartmentId")]Course course)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _courseService.Add(course);
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            await PopulateDepartmentDropDown(course.DepartmentId);
            return View(course);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var course = await _courseService.Get(id.Value);
            if (course == null)
                return HttpNotFound();

            await PopulateDepartmentDropDown(course.DepartmentId);
            return View(course);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var course = await _courseService.Get(id.Value);
            var fieldsToBind = new[] { "Title", "Credits", "DepartmentId" };
            if (TryUpdateModel(course, fieldsToBind))
            {
                try
                {
                    await _courseService.Update(id.Value, course);
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            await PopulateDepartmentDropDown(course.DepartmentId);
            return View(course);
        }

        // GET: Course/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var course = await _courseService.Get(id.Value);
            if (course == null)
                return HttpNotFound();

            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _courseService.Delete(id);
                return RedirectToAction("Index");
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            var course = await _courseService.Get(id);
            return View(course);
        }

        private async Task PopulateDepartmentDropDown(int? departmentId)
        {
            ViewBag.DepartmentId = new SelectList(await _departmentService.Get(), "DepartmentId", "Name", departmentId);
        }
    }
}
