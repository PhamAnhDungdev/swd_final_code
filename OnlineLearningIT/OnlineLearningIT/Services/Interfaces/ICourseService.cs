using OnlineLearningIT.Models;
namespace OnlineLearningIT.Services.Interfaces
{
    public interface ICourseService
    {
        Task<bool> UpdateCourse(Course course);

        Task<bool> ValidateCourse(Course course);

        Task<List<Course>> GetListCourse();
    }
}
