using OnlineCoursesSubscription.Models;

namespace Courses.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> CreateAsync(User user);
        Task<bool> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
        Task<bool> IsEmailUniqueAsync(string email, int? userId = null);
        Task<bool> CanSubscribeAsync(int userId);
    }
}
