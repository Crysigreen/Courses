using Microsoft.EntityFrameworkCore;
using OnlineCoursesSubscription.Models;

namespace Courses.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public SubscriptionService(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<IEnumerable<subscription>> GetAllAsync()
        {
            return await _context.subscriptions.ToListAsync();
        }

        public async Task<subscription> GetByIdAsync(int id)
        {
            return await _context.subscriptions.FindAsync(id);
        }

        public async Task<IEnumerable<subscription>> GetByUserIdAsync(int userId)
        {
            return await _context.subscriptions
                .Where(s => s.UserId == userId)
                .Include(s => s.Course)
                .ToListAsync();
        }

        public async Task<subscription> CreateAsync(subscription subscription)
        {
            // Проверка на существование пользователя и курса
            if (!await _context.users.AnyAsync(u => u.Id == subscription.UserId))
                throw new Exception($"Пользователь с Id {subscription.UserId} не найден.");

            if (!await _context.courses.AnyAsync(c => c.Id == subscription.CourseId))
                throw new Exception($"Курс с Id {subscription.CourseId} не найден.");

            // Проверка на существующую подписку
            if (await _context.subscriptions.AnyAsync(s => s.UserId == subscription.UserId && s.CourseId == subscription.CourseId))
                throw new Exception("Подписка уже существует.");

            // Проверка на максимальное количество подписок
            if (!await _userService.CanSubscribeAsync(subscription.UserId))
                throw new Exception("Достигнуто максимальное количество подписок для пользователя.");

            subscription.SubscribedOn = DateTime.UtcNow;

            _context.subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            return subscription;
        }

        public async Task<bool> UpdateAsync(subscription subscription)
        {
            var existingSubscription = await _context.subscriptions.FindAsync(subscription.Id);
            if (existingSubscription == null)
                return false;

            existingSubscription.UserId = subscription.UserId;
            existingSubscription.CourseId = subscription.CourseId;
            existingSubscription.SubscribedOn = subscription.SubscribedOn;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var subscription = await _context.subscriptions.FindAsync(id);
            if (subscription == null)
                return false;

            _context.subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
