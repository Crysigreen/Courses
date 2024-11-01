using Courses.GraphQL.Queries;
using OnlineCoursesSubscription.Models;

namespace Courses.GraphQL.Mutations
{
    public class SubscriptionMutation
    {
        private readonly ICourseStorage _db;

        public SubscriptionMutation(ICourseStorage db)
        {
            _db = db;
        }

        public async Task<subscription> CreateSubscription(int userId, int courseId)
        {
            var subscription = new subscription
            {
                UserId = userId,
                CourseId = courseId,
                SubscribedOn = DateTime.UtcNow
            };
            await _db.CreateSubscriptionAsync(subscription);
            return subscription;
        }
    }
}
