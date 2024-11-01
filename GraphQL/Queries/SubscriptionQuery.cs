using OnlineCoursesSubscription.Models;

namespace Courses.GraphQL.Queries
{
    public class SubscriptionQuery
    {
        private readonly ICourseStorage _db;

        public SubscriptionQuery(ICourseStorage db)
        {
            _db = db;
        }

        [UseFiltering]
        [UseSorting]
        public IEnumerable<subscription> GetSubscriptions() => _db.ListSubscriptions();

        public subscription GetSubscriptionById(int id) => _db.FindSubscription(id);
    }
}
