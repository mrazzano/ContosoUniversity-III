using System.Net;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using ContosoUniversity.Core.Models;
using ContosoUniversity.Service;

namespace ContosoUniversity.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly DepartmentService _departmentService;
        private readonly InstructorService _instructorService;

        public DepartmentController(DepartmentService departmentService, InstructorService instructorService)
        {
            _departmentService = departmentService;
            _instructorService = instructorService;
        }

        // GET: Department
        public async Task<ActionResult> Index()
        {
            var departments = await _departmentService.Get();
            return View(departments);
        }

        // GET: Department/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var department = await _departmentService.Get(id.Value);
            if (department == null)
                return HttpNotFound();

            return View(department);
        }

        // GET: Department/Create
        public async Task<ActionResult> Create()
        {
            await PopulateInstructorDropdown(null);
            return View();
        }

        // POST: Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DepartmentId,Name,Budget,StartDate,InstructorId")] Department department)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _departmentService.Add(department);
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            await PopulateInstructorDropdown(department.InstructorId);
            return View(department);
        }

        // GET: Department/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var department = await _departmentService.Get(id.Value);
            if (department == null)
                return HttpNotFound();

            await PopulateInstructorDropdown(department.InstructorId);
            return View(department);
        }

        // POST: Department/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var department = await _departmentService.Get(id.Value);
            string[] fieldsToBind = { "Name", "Budget", "StartDate", "InstructorId" };
            if (TryUpdateModel(department, fieldsToBind))
            {
                try
                {
                    await _departmentService.Update(id.Value, department);
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            await PopulateInstructorDropdown(department.InstructorId);
            return View(department);
        }

        // GET: Department/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var department = await _departmentService.Get(id.Value);
            if (department == null)
                return HttpNotFound();

            return View(department);
        }

        // POST: Department/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var department = await _departmentService.Get(id);
            try
            {
                await _departmentService.Delete(id);
                return RedirectToAction("Index");
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(department);
        }

        private async Task PopulateInstructorDropdown(int? instructorId)
        {
            ViewBag.InstructorId = new SelectList(await _instructorService.Get(), "Id", "FullName", instructorId);
        }
    }
}
