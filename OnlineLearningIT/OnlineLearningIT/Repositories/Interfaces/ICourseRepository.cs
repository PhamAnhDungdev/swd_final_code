using OnlineLearningIT.Models;
namespace OnlineLearningIT.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<bool> UpdateCourse(Course course);

        Task<bool> ValidateCourse(Course course);
    }
}
