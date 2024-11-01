using OnlineCoursesSubscription.Models;

namespace Courses.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<cours>> GetAllAsync();
        Task<cours> GetByIdAsync(int id);
        Task<cours> CreateAsync(cours course);
        Task<bool> UpdateAsync(cours course);
        Task<bool> DeleteAsync(int id);
    }
}
