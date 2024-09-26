using Microsoft.EntityFrameworkCore;
using OnlineCoursesSubscription.Models;

namespace Courses.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private const int MaxSubscriptionsPerUser = 5; // Бизнес-правило: максимум 5 подписок на пользователя

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> CreateAsync(User user)
        {
            // Проверка на уникальность email
            if (!await IsEmailUniqueAsync(user.Email))
                throw new Exception("Пользователь с таким email уже существует.");

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
                return false;

            // Проверка на уникальность email
            if (!await IsEmailUniqueAsync(user.Email, user.Id))
                throw new Exception("Пользователь с таким email уже существует.");

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.Include(u => u.Subscriptions).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return false;

            // Удаляем связанные подписки
            if (user.Subscriptions.Any())
            {
                _context.Subscriptions.RemoveRange(user.Subscriptions);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsEmailUniqueAsync(string email, int? userId = null)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return true;

            if (userId.HasValue && user.Id == userId.Value)
                return true;

            return false;
        }

        public async Task<bool> CanSubscribeAsync(int userId)
        {
            var subscriptionCount = await _context.Subscriptions.CountAsync(s => s.UserId == userId);
            return subscriptionCount < MaxSubscriptionsPerUser;
        }
    }
}
