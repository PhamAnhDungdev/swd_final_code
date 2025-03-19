using OnlineLearningIT.Services.Interfaces;
using OnlineLearningIT.Models;
using OnlineLearningIT.Repositories.Interfaces;

namespace OnlineLearningIT.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public Task<List<Course>> GetListCourse()
        {
            return _courseRepository.GetListCourse();
        }

        public Task<bool> UpdateCourse(Course course)
        {
            return _courseRepository.UpdateCourse(course);
        }

        public Task<bool> ValidateCourse(Course course)
        {
            return _courseRepository.ValidateCourse(course);
        }
    }
}
