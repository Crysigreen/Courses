using OnlineCoursesSubscription.Models;

namespace Courses.GraphQL.Queries
{
    public class CourseQuery
    {
        private readonly ICourseStorage _db;

        public CourseQuery(ICourseStorage db)
        {
            _db = db;
        }

        [UseFiltering]
        [UseSorting]
        public IEnumerable<cours> GetCourses() => _db.ListCourses();

        public cours GetCourseById(int id) => _db.FindCourse(id);
    }
}
