using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineLearningIT.Models;
namespace OnlineLearningIT.Controllers.CertificateController
{
    public class CertificateController : Controller
    {
        private OnlineLearningItContext _context;
        public CertificateController(OnlineLearningItContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var course = _context.Certificates.ToList();
            return View(course);
        }

        public ActionResult Edit(int id)
        {
            var course = _context.Certificates.Where(c => c.CertificateId == id).FirstOrDefault();
            return View(course);
        }

        public ActionResult Create()
        {
            var courses = _context.Courses.ToList();
            ViewBag.Course = courses;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Certificate certificate)
        {
            _context.Certificates.Add(certificate);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Thêm mới chứng chỉ thành công!";
            return RedirectToAction("Index", "Certificate");
        }
    }
}
