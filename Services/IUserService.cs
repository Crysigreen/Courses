using OnlineCoursesSubscription.Models;

namespace Courses.Services
{
    public interface IUserService
    {
        Task<IEnumerable<users>> GetAllAsync();
        Task<users> GetByIdAsync(int id);
        Task<users> CreateAsync(users user);
        Task<bool> UpdateAsync(users user);
        Task<bool> DeleteAsync(int id);
        Task<bool> IsEmailUniqueAsync(string email, int? userId = null);
        Task<bool> CanSubscribeAsync(int userId);
    }
}
