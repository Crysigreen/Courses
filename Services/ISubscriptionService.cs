using OnlineCoursesSubscription.Models;

namespace Courses.Services
{
    public interface ISubscriptionService
    {
        Task<IEnumerable<Subscription>> GetAllAsync();
        Task<Subscription> GetByIdAsync(int id);
        Task<IEnumerable<Subscription>> GetByUserIdAsync(int userId);
        Task<Subscription> CreateAsync(Subscription subscription);
        Task<bool> UpdateAsync(Subscription subscription);
        Task<bool> DeleteAsync(int id);
    }
}
