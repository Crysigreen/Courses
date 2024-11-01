using Courses.GraphQL.Queries;
using OnlineCoursesSubscription.Models;

namespace Courses.GraphQL.Mutations
{
    public class CourseMutation
    {
        private readonly ICourseStorage _db;

        public CourseMutation(ICourseStorage db)
        {
            _db = db;
        }

        public async Task<cours> CreateCourse(string title, string description)
        {
            var course = new cours
            {
                Title = title,
                Description = description
            };
            await _db.CreateCourseAsync(course);
            return course;
        }
    }
}
