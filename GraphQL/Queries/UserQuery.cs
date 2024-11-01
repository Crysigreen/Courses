using OnlineCoursesSubscription.Models;

namespace Courses.GraphQL.Queries
{
    public class UserQuery
    {
        private readonly ICourseStorage _db;

        public UserQuery(ICourseStorage db)
        {
            _db = db;
        }

        [UseFiltering]
        [UseSorting]
        public IEnumerable<users> GetUsers() => _db.ListUsers();

        public users GetUserById(int id) => _db.FindUser(id);
    }
}
