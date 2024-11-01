using Courses.GraphQL.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OnlineCoursesSubscription.Models;

namespace Courses.GraphQL.Mutations
{
    public class Mutation
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public Mutation(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<users> CreateUser(string name, string email)
        {
            using var context = _contextFactory.CreateDbContext();
            var user = new users { Name = name, Email = email };
            context.users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<cours> CreateCourse(string title, string description)
        {
            using var context = _contextFactory.CreateDbContext();
            var course = new cours { Title = title, Description = description };
            context.courses.Add(course);
            await context.SaveChangesAsync();
            return course;
        }

        public async Task<subscription> CreateSubscription(int userId, int courseId)
        {
            using var context = _contextFactory.CreateDbContext();

            var existingSubscription = await context.subscriptions
                                                    .AsNoTracking()
                                                    .FirstOrDefaultAsync(s => s.UserId == userId && s.CourseId == courseId);

            if (existingSubscription != null)
            {
                throw new Exception("Пользователь уже подписан на этот курс.");
            }

            var course = await context.courses.AsNoTracking().FirstOrDefaultAsync(c => c.Id == courseId);
            if (course == null)
            {
                throw new Exception("Курс не найден.");
            }

            var subscription = new subscription
            {
                UserId = userId,
                CourseId = courseId,
                SubscribedOn = DateTime.UtcNow
            };

            context.subscriptions.Add(subscription);
            await context.SaveChangesAsync();
            return subscription;
        }
    }
}
