using Microsoft.EntityFrameworkCore;
using OnlineCoursesSubscription.Models;

namespace Courses.GraphQL.Queries
{

    public class Query
    {
        private readonly IDbContextFactory<ApplicationDbContext> _context;

        public Query(IDbContextFactory<ApplicationDbContext> context)
        {
            _context = context;
        }

        [UseFiltering]
        [UseSorting]
        public IEnumerable<users> GetUsers()
        {
            using var context = _context.CreateDbContext();
            return context.users
                           .AsNoTracking()
                           .Include(u => u.Subscriptions)
                           .ThenInclude(s => s.Course)
                           .ToList();
        }

        [UseFiltering]
        [UseSorting]
        public IEnumerable<cours> GetCourses()
        {
            using var context = _context.CreateDbContext();
            return context.courses
                           .AsNoTracking()
                           .Include(c => c.Subscriptions)
                           .ThenInclude(s => s.User)
                           .ToList();
        }

        [UseFiltering]
        [UseSorting]
        public IEnumerable<subscription> GetSubscriptions()
        {
            using var context = _context.CreateDbContext();
            return context.subscriptions
                           .AsNoTracking()
                           .Include(s => s.User)
                           .Include(s => s.Course)
                           .ToList();
        }

        // Получение курсов по пользователю
        public IEnumerable<cours> GetCoursesByUser(int userId)
        {
            using var context = _context.CreateDbContext();
            var subscriptions = context.subscriptions
                                        .AsNoTracking()
                                        .Where(s => s.UserId == userId)
                                        .Include(s => s.Course)
                                        .ToList();

            return subscriptions.Select(s => s.Course).Distinct();
        }

        // Получение пользователей по курсу
        public IEnumerable<users> GetUsersByCourse(int courseId)
        {
            using var context = _context.CreateDbContext();
            var subscriptions = context.subscriptions
                                        .AsNoTracking()
                                        .Where(s => s.CourseId == courseId)
                                        .Include(s => s.User)
                                        .ToList();

            return subscriptions.Select(s => s.User).Distinct();
        }

        // Недавние подписки
        public IEnumerable<subscription> GetRecentSubscriptions()
        {
            using var context = _context.CreateDbContext();
            var lastMonth = DateTime.UtcNow.AddMonths(-1);
            return context.subscriptions
                           .AsNoTracking()
                           .Where(s => s.SubscribedOn >= lastMonth)
                           .Include(s => s.User)
                           .Include(s => s.Course)
                           .ToList();
        }
    }
}
