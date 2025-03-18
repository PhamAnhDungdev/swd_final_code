using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLearningIT.Models;

namespace OnlineLearningIT.Controllers.CourseController
{
    public class CourseController : Controller
    {

        private OnlineLearningItContext _context;
        public CourseController(OnlineLearningItContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var course = _context.Courses.ToList();
            return View(course);
        }

        public ActionResult Edit(int id) { 
            var course = _context.Courses.Where(c => c.CourseId == id).FirstOrDefault();
            return View(course); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(course).State = EntityState.Modified;
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Cập nhật khóa học thành công!";
                    return RedirectToAction("Index", "Course");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Không thể cập nhật khóa học. Lỗi: " + ex.Message);
                }
            }
            return View(course);
        }
    }
}
