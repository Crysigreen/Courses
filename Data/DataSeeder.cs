using OnlineCoursesSubscription.Models;

namespace OnlineCoursesSubscription.Data
{
    public static class DataSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Убедитесь, что база данных создана
            context.Database.EnsureCreated();

            // Проверка наличия данных
            if (!context.Users.Any())
            {
                // Добавление пользователей
                context.Users.AddRange(
                    new User { Id = 1, Name = "Алиса Иванова", Email = "alice@example.com" },
                    new User { Id = 2, Name = "Борис Смирнов", Email = "boris@example.com" },
                    new User { Id = 3, Name = "Виктор Петров", Email = "victor@example.com" }
                );
                context.SaveChanges();
            }

            if (!context.Courses.Any())
            {
                // Добавление курсов
                context.Courses.AddRange(
                    new Course { Id = 1, Title = "Введение в C#", Description = "Основы программирования на C#." },
                    new Course { Id = 2, Title = "Продвинутый ASP.NET Core", Description = "Глубокое погружение в ASP.NET Core." },
                    new Course { Id = 3, Title = "Глубокое изучение Entity Framework Core", Description = "Изучение EF Core и практики использования." }
                );
                context.SaveChanges();
            }

            if (!context.Subscriptions.Any())
            {
                // Добавление подписок
                context.Subscriptions.AddRange(
                    new Subscription
                    {
                        Id = 1,
                        UserId = 1,
                        CourseId = 1,
                        SubscribedOn = DateTime.SpecifyKind(new DateTime(2023, 10, 1), DateTimeKind.Utc)
                    },
                    new Subscription
                    {
                        Id = 2,
                        UserId = 1,
                        CourseId = 2,
                        SubscribedOn = DateTime.SpecifyKind(new DateTime(2023, 10, 2), DateTimeKind.Utc)
                    },
                    new Subscription
                    {
                        Id = 3,
                        UserId = 2,
                        CourseId = 2,
                        SubscribedOn = DateTime.SpecifyKind(new DateTime(2023, 10, 3), DateTimeKind.Utc)
                    },
                    new Subscription
                    {
                        Id = 4,
                        UserId = 3,
                        CourseId = 3,
                        SubscribedOn = DateTime.SpecifyKind(new DateTime(2023, 10, 4), DateTimeKind.Utc)
                    });
                    context.SaveChanges();
            }
        }
    }
}
