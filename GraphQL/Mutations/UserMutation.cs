using Courses.GraphQL.Queries;
using OnlineCoursesSubscription.Models;

namespace Courses.GraphQL.Mutations
{
    public class UserMutation
    {
        private readonly ICourseStorage _db;

        public UserMutation(ICourseStorage db)
        {
            _db = db;
        }

        //public async Task<users> CreateUser(string name, string email)
        //{
        //    var user = new users
        //    {
        //        Name = name,
        //        Email = email
        //    };
        //    await _db.CreateUserAsync(user);
        //    return user;
        //}
    }
}
