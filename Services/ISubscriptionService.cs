using OnlineCoursesSubscription.Models;

namespace Courses.Services
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<subscription>> GetAllAsync();
        Task<subscription> GetByIdAsync(int id);
        Task<IEnumerable<subscription>> GetByUserIdAsync(int userId);
        Task<subscription> CreateAsync(subscription subscription);
        Task<bool> UpdateAsync(subscription subscription);
        Task<bool> DeleteAsync(int id);
    }
}
