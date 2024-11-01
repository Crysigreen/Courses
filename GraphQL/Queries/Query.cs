using Microsoft.EntityFrameworkCore;
using OnlineCoursesSubscription.Models;

namespace Courses.GraphQL.Queries
{

    public class Query
    {
        private readonly ApplicationDbContext _context;

        public Query(ApplicationDbContext context)
        {
            _context = context;
        }

        [UseFiltering]
        [UseSorting]
        public IEnumerable<users> GetUsers()
        {
            return _context.users
                           .AsNoTracking()
                           .Include(u => u.Subscriptions)
                           .ThenInclude(s => s.Course)
                           .ToList();
        }

        [UseFiltering]
        [UseSorting]
        public IEnumerable<cours> GetCourses()
        {
            return _context.courses
                           .AsNoTracking()
                           .Include(c => c.Subscriptions)
                           .ThenInclude(s => s.User)
                           .ToList();
        }

        [UseFiltering]
        [UseSorting]
        public IEnumerable<subscription> GetSubscriptions()
        {
            return _context.subscriptions
                           .AsNoTracking()
                           .Include(s => s.User)
                           .Include(s => s.Course)
                           .ToList();
        }

        // Получение курсов по пользователю
        public IEnumerable<cours> GetCoursesByUser(int userId)
        {
            var subscriptions = _context.subscriptions
                                        .AsNoTracking()
                                        .Where(s => s.UserId == userId)
                                        .Include(s => s.Course)
                                        .ToList();

            return subscriptions.Select(s => s.Course).Distinct();
        }

        // Получение пользователей по курсу
        public IEnumerable<users> GetUsersByCourse(int courseId)
        {
            var subscriptions = _context.subscriptions
                                        .AsNoTracking()
                                        .Where(s => s.CourseId == courseId)
                                        .Include(s => s.User)
                                        .ToList();

            return subscriptions.Select(s => s.User).Distinct();
        }

        // Недавние подписки
        public IEnumerable<subscription> GetRecentSubscriptions()
        {
            var lastMonth = DateTime.UtcNow.AddMonths(-1);
            return _context.subscriptions
                           .AsNoTracking()
                           .Where(s => s.SubscribedOn >= lastMonth)
                           .Include(s => s.User)
                           .Include(s => s.Course)
                           .ToList();
        }
    }
}
