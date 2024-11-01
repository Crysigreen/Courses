using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<users> ListUsers()
        {
            return _context.users
                           .AsNoTracking()
                           .Include(u => u.Subscriptions)
                           .ThenInclude(s => s.Course)
                           .ToList();  // Завершает запрос
        }

        public users FindUser(int id)
        {
            return _context.users
                           .AsNoTracking()
                           .Include(u => u.Subscriptions)
                           .ThenInclude(s => s.Course)
                           .FirstOrDefault(u => u.Id == id);  // Завершает запрос
        }

        public IEnumerable<cours> ListCourses()
        {
            return _context.courses
                           .AsNoTracking()
                           .Include(c => c.Subscriptions)
                           .ThenInclude(s => s.User)
                           .ToList();  // Завершает запрос
        }

        public cours FindCourse(int id)
        {
            return _context.courses
                           .AsNoTracking()
                           .Include(c => c.Subscriptions)
                           .ThenInclude(s => s.User)
                           .FirstOrDefault(c => c.Id == id);  // Завершает запрос
        }

        public IEnumerable<subscription> ListSubscriptions()
        {
            return _context.subscriptions
                           .AsNoTracking()
                           .Include(s => s.User)
                           .Include(s => s.Course)
                           .ToList();  // Завершает запрос
        }

        public subscription FindSubscription(int id)
        {
            return _context.subscriptions
                           .AsNoTracking()
                           .Include(s => s.User)
                           .Include(s => s.Course)
                           .FirstOrDefault(s => s.Id == id);  // Завершает запрос
        }
    }



}
