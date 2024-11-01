using Courses.GraphQL.Queries;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesSubscription.Models;

namespace Courses.GraphQL.Mutations
{
    public class Mutation
    {
        private readonly ApplicationDbContext _context;

        public Mutation(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<users> CreateUser(string name, string email)
        {
            var user = new users { Name = name, Email = email };
            _context.users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<cours> CreateCourse(string title, string description)
        {
            var course = new cours { Title = title, Description = description };
            _context.courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<subscription> CreateSubscription(int userId, int courseId)
        {
            // Проверка: существует ли уже подписка на этот курс для пользователя
            var existingSubscription = _context.subscriptions
                                              .AsNoTracking()
                                              .FirstOrDefault(s => s.UserId == userId && s.CourseId == courseId);

            if (existingSubscription != null)
            {
                throw new Exception("Пользователь уже подписан на этот курс.");
            }

            // Проверка: существует ли указанный курс
            var course = _context.courses.AsNoTracking().FirstOrDefault(c => c.Id == courseId);
            if (course == null)
            {
                throw new Exception("Курс не найден.");
            }

            // Создание новой подписки
            var subscription = new subscription
            {
                UserId = userId,
                CourseId = courseId,
                SubscribedOn = DateTime.UtcNow
            };

            _context.subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            return subscription;
        }
    }
}
