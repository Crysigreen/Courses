using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OnlineCoursesSubscription.Models;
using System;

namespace Courses.GraphQL.Queries
{
    public class CourseStorage : ICourseStorage
    {
        private readonly IDbContextFactory<ApplicationDbContext> _context;

        public CourseStorage(IDbContextFactory<ApplicationDbContext> context)
        {
            _context = context;
        }

        public IEnumerable<users> ListUsers()
        {
            using var context = _context.CreateDbContext();
            return context.users
                           .AsNoTracking()
                           .Include(u => u.Subscriptions)
                           .ThenInclude(s => s.Course)
                           .ToList();  // Завершает запрос
        }

        public users FindUser(int id)
        {
            using var context = _context.CreateDbContext();
            return context.users
                           .AsNoTracking()
                           .Include(u => u.Subscriptions)
                           .ThenInclude(s => s.Course)
                           .FirstOrDefault(u => u.Id == id);  // Завершает запрос
        }

        public IEnumerable<cours> ListCourses()
        {
            using var context = _context.CreateDbContext();
            return context.courses
                           .AsNoTracking()
                           .Include(c => c.Subscriptions)
                           .ThenInclude(s => s.User)
                           .ToList();  // Завершает запрос
        }

        public cours FindCourse(int id)
        {
            using var context = _context.CreateDbContext();
            return context.courses
                           .AsNoTracking()
                           .Include(c => c.Subscriptions)
                           .ThenInclude(s => s.User)
                           .FirstOrDefault(c => c.Id == id);  // Завершает запрос
        }

        public IEnumerable<subscription> ListSubscriptions()
        {
            using var context = _context.CreateDbContext();
            return context.subscriptions
                           .AsNoTracking()
                           .Include(s => s.User)
                           .Include(s => s.Course)
                           .ToList();  // Завершает запрос
        }

        public subscription FindSubscription(int id)
        {
            using var context = _context.CreateDbContext();
            return context.subscriptions
                           .AsNoTracking()
                           .Include(s => s.User)
                           .Include(s => s.Course)
                           .FirstOrDefault(s => s.Id == id);  // Завершает запрос
        }
    }



}
