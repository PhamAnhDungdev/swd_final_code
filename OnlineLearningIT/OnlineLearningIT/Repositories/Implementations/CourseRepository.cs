using Microsoft.EntityFrameworkCore;
using OnlineLearningIT.Models;
using OnlineLearningIT.Repositories.Interfaces;

namespace OnlineLearningIT.Repositories.Implementations
{
    public class CourseRepository : ICourseRepository
    {

        private OnlineLearningItContext _context;

        public CourseRepository(OnlineLearningItContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetListCourse()
        {
            var listCourse = _context.Courses.ToList();
            return listCourse;
        }

        public async Task<bool> UpdateCourse(Course course)
        {
            var cOld = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == course.CourseId);
            if (cOld != null)
            {
                _context.Entry(cOld).CurrentValues.SetValues(course);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ValidateCourse(Course course)
        {
            if (course == null)
            {
                return false;
            }
            if (string.IsNullOrWhiteSpace(course.Title) || course.Title.Length > 100)
            {
                return false;
            }
            if (course.Description != null && course.Description.Length > 500)
            {
                return false;
            }

            if (course.ImageUrl != null && !Uri.TryCreate(course.ImageUrl, UriKind.Absolute, out _))
            {
                return false;
            }

            if (course.Duration.HasValue && course.Duration.Value <= 0)
            {
                return false;
            }

            if (course.Price.HasValue && course.Price.Value < 0)
            {
                return false;
            }

            string[] validLevels = { "Beginner", "Intermediate", "Advanced", "All Levels" };
            if (course.Level != null && !validLevels.Contains(course.Level))
            {
                return false;
            }

            return true;
        }
    }
}
