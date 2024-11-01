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
            if (!context.users.Any())
            {
                // Добавление пользователей
                context.users.AddRange(
                    new users { Id = 1, Name = "Алиса Иванова", Email = "alice@example.com" },
                    new users { Id = 2, Name = "Борис Смирнов", Email = "boris@example.com" },
                    new users { Id = 3, Name = "Виктор Петров", Email = "victor@example.com" }
                );
                context.SaveChanges();
            }

            if (!context.courses.Any())
            {
                // Добавление курсов
                context.courses.AddRange(
                    new cours { Id = 1, Title = "Введение в C#", Description = "Основы программирования на C#." },
                    new cours { Id = 2, Title = "Продвинутый ASP.NET Core", Description = "Глубокое погружение в ASP.NET Core." },
                    new cours { Id = 3, Title = "Глубокое изучение Entity Framework Core", Description = "Изучение EF Core и практики использования." }
                );
                context.SaveChanges();
            }

            if (!context.subscriptions.Any())
            {
                // Добавление подписок
                context.subscriptions.AddRange(
                    new subscription
                    {
                        Id = 1,
                        UserId = 1,
                        CourseId = 1,
                        SubscribedOn = DateTime.SpecifyKind(new DateTime(2023, 10, 1), DateTimeKind.Utc)
                    },
                    new subscription
                    {
                        Id = 2,
                        UserId = 1,
                        CourseId = 2,
                        SubscribedOn = DateTime.SpecifyKind(new DateTime(2023, 10, 2), DateTimeKind.Utc)
                    },
                    new subscription
                    {
                        Id = 3,
                        UserId = 2,
                        CourseId = 2,
                        SubscribedOn = DateTime.SpecifyKind(new DateTime(2023, 10, 3), DateTimeKind.Utc)
                    },
                    new subscription
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
