using System;
using System.Net;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using ContosoUniversity.Core.Models;
using ContosoUniversity.Service;
using PagedList;

namespace ContosoUniversity.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: Student
        public async Task<ViewResult> Index(string sortOrder, string currentFilter, string searchValue, int? page)
        {
            const int pageSize = 5;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.FirstNameSortParm = sortOrder == "firstname" ? "firstname_desc" : "firstname";
            ViewBag.LastNameSortParm = sortOrder == "lastname" ? "lastname_desc" : "lastname";
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";

            if (searchValue != null)
            {
                page = 1;
            }
            else
            {
                searchValue = currentFilter;
            }

            ViewBag.CurrentFilter = searchValue;


            var students = await _studentService.Get();
            if (!String.IsNullOrEmpty(searchValue))
            {
                students = students.Where(s =>
                    s.FirstMidName.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >=0 || 
                    s.LastName.IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) >=0);
            }
            students = Sort(students, sortOrder);

            var pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));
        }

        // GET: Student/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var student = await _studentService.Get(id.Value);
            if (student == null)
                return HttpNotFound();

            return View(student);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "LastName, FirstMidName, EnrollmentDate")]Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _studentService.Add(student);
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(student);
        }

        // GET: Student/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var student = await _studentService.Get(id.Value);
            if (student == null)
                return HttpNotFound();

            return View(student);
        }

        // POST: Student/Edit/5
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var student = await _studentService.Get(id.Value);
            var fieldsToBind = new[] { "LastName", "FirstMidName", "EnrollmentDate" };
            if (TryUpdateModel(student, fieldsToBind))
            {
                try
                {
                    await _studentService.Update(id.Value, student);
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(student);
        }

        // GET: Student/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var student = await _studentService.Get(id.Value);
            if (student == null)
                return HttpNotFound();

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _studentService.Delete(id);
                return RedirectToAction("Index");
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            var student = await _studentService.Get(id);
            return View(student);
        }

        private static IEnumerable<Student> Sort(IEnumerable<Student> students, string sortOrder)
        {
            switch (sortOrder)
            {
                case "lastname_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "firstname":
                    students = students.OrderBy(s => s.FirstMidName);
                    break;
                case "firstname_desc":
                    students = students.OrderByDescending(s => s.FirstMidName);
                    break;
                case "date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default: // Last Name Ascending 
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            return students;
        }
    }
}
