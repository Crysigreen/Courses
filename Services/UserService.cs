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

        public async Task<IEnumerable<users>> GetAllAsync()
        {
            return await _context.users.ToListAsync();
        }

        public async Task<users> GetByIdAsync(int id)
        {
            return await _context.users.FindAsync(id);
        }

        public async Task<users> CreateAsync(users user)
        {
            // Проверка на уникальность email
            if (!await IsEmailUniqueAsync(user.Email))
                throw new Exception("Пользователь с таким email уже существует.");

            _context.users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateAsync(users user)
        {
            var existingUser = await _context.users.FindAsync(user.Id);
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
            var user = await _context.users.Include(u => u.Subscriptions).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return false;

            // Удаляем связанные подписки
            if (user.Subscriptions.Any())
            {
                _context.subscriptions.RemoveRange(user.Subscriptions);
            }

            _context.users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsEmailUniqueAsync(string email, int? userId = null)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return true;

            if (userId.HasValue && user.Id == userId.Value)
                return true;

            return false;
        }

        public async Task<bool> CanSubscribeAsync(int userId)
        {
            var subscriptionCount = await _context.subscriptions.CountAsync(s => s.UserId == userId);
            return subscriptionCount < MaxSubscriptionsPerUser;
        }
    }
}
