using Microsoft.AspNetCore.Mvc;
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
    }
}
