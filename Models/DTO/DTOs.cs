namespace Courses.Models.DTO
{
    public class DTOs
    {



        public class CreateCourseDTO
        {
            public string Title { get; set; }
            public string Description { get; set; }
        }

        public class CreateUserDTO
        {
            public string Name { get; set; }
            public string Email { get; set; }
        }

        public class CreateSubscribeDTO
        {
            public int UserId { get; set; }
            public int CourseId { get; set; }

        }

    }
}
