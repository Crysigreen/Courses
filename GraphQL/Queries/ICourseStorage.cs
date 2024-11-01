using OnlineCoursesSubscription.Models;

namespace Courses.GraphQL.Queries
{
    public interface ICourseStorage
    {
        // Методы для чтения
        IEnumerable<users> ListUsers();
        users FindUser(int id);

        IEnumerable<cours> ListCourses();
        cours FindCourse(int id);

        IEnumerable<subscription> ListSubscriptions();
        subscription FindSubscription(int id);

        
    }

}
