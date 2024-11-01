using OnlineCoursesSubscription.Models;
using System;

namespace Courses.GraphQL.Queries
{
    public class CourseStorage : ICourseStorage
    {
        private readonly ApplicationDbContext _context;

        public CourseStorage(ApplicationDbContext context)
        {
            _context = context;
        }

        // Методы для чтения
        public IEnumerable<users> ListUsers() => _context.users.ToList();
        public users FindUser(int id) => _context.users.Find(id);

        public IEnumerable<cours> ListCourses() => _context.courses.ToList();
        public cours FindCourse(int id) => _context.courses.Find(id);

        public IEnumerable<subscription> ListSubscriptions() => _context.subscriptions.ToList();
        public subscription FindSubscription(int id) => _context.subscriptions.Find(id);

        // Методы для создания
        public async Task CreateUserAsync(users user)
        {
            _context.users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task CreateCourseAsync(cours course)
        {
            _context.courses.Add(course);
            await _context.SaveChangesAsync();
        }

        public async Task CreateSubscriptionAsync(subscription subscription)
        {
            _context.subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
        }
    }

}
