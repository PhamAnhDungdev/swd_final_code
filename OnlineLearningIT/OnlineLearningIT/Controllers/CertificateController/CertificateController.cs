using Microsoft.AspNetCore.Mvc;
using OnlineLearningIT.Models;
using OnlineLearningIT.Services.Interfaces;
namespace OnlineLearningIT.Controllers.CertificateController
{
    public class CertificateController : Controller
    {
        private OnlineLearningItContext _context;
        private readonly ICertificateService _certificateService;
        public CertificateController(OnlineLearningItContext context, ICertificateService certificateService)
        {
            _context = context;
            _certificateService = certificateService;
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
        public async Task<ActionResult> Create(Certificate certificate)
        {

            if (await _certificateService.ValidateCertificate(certificate))
            {
                var result = await _certificateService.AddNewCertificate(certificate);
                if (result)
                {
                    TempData["SuccessMessage"] = "Thêm mới chứng chỉ thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Thêm mới chứng chỉ thất bại!";
                    return RedirectToAction("Create", "Certificate");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Input chứng chỉ không hợp lệ!";
                return RedirectToAction("Create", "Certificate");
            }
            return RedirectToAction("Index", "Certificate");
        }
    }
}
