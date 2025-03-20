using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineLearningIT.Models;
using OnlineLearningIT.Services.Interfaces;

namespace OnlineLearningIT.Controllers.CourseController
{
    public class CourseController : Controller
    {

        private readonly OnlineLearningItContext _context;

        private readonly ICourseService _courseService;
        public CourseController(OnlineLearningItContext context, ICourseService courseService)
        {
            _context = context;
            _courseService = courseService;
        }

        public async Task<ActionResult> Index()
        {
            var course = await _courseService.GetListCourse();
            return View(course);
        }

        public ActionResult Edit(int id)
        {
            var course = _context.Courses.Where(c => c.CourseId == id).FirstOrDefault();
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var valid = await _courseService.ValidateCourse(course);
                    if (valid)
                    {
                        var result = await _courseService.UpdateCourse(course);
                        if (result)
                        {
                            TempData["SuccessMessage"] = "Cập nhật khóa học thành công!";
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Cập nhật khóa học thất bại!";
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Input không hợp lệ, xem xét lại các giá trị";
                    }

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
