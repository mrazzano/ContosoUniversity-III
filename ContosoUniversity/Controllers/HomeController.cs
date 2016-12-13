using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using ContosoUniversity.Core.ViewModels;
using ContosoUniversity.Service;

namespace ContosoUniversity.Controllers
{
    public class HomeController : Controller
    {
        private readonly StudentService _studentService;

        public HomeController(StudentService studentService)
        {
            _studentService = studentService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Enrollment()
        {
            var students = await _studentService.Get();
            var enrollments = students
              .GroupBy(x => x.EnrollmentDate)
              .Select(g => new EnrollmentDateViewModel
              {
                  EnrollmentDate = g.Key, 
                  StudentCount = g.Count()
              }).ToList();

            return View(enrollments);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}