using Microsoft.EntityFrameworkCore;

namespace OnlineCoursesSubscription.Models
{
    public class ApplicationDbContext : DbContext
    {
        // DbSet
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Настройка моделей и начальные данные
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка первичных ключей и связей
            modelBuilder.Entity<Subscription>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithMany(u => u.Subscriptions)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Course)
                .WithMany(c => c.Subscriptions)
                .HasForeignKey(s => s.CourseId);

            // Настройка свойств
            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Course>()
                .Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Course>()
                .Property(c => c.Description)
                .HasMaxLength(1000);

            modelBuilder.Entity<Subscription>()
                .Property(s => s.SubscribedOn)
                .IsRequired();

            // Начальное заполнение данных
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Алиса Иванова", Email = "alice@example.com" },
                new User { Id = 2, Name = "Борис Смирнов", Email = "boris@example.com" },
                new User { Id = 3, Name = "Виктор Петров", Email = "victor@example.com" }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Title = "Введение в C#", Description = "Основы программирования на C#." },
                new Course { Id = 2, Title = "Продвинутый ASP.NET Core", Description = "Глубокое погружение в ASP.NET Core." },
                new Course { Id = 3, Title = "Глубокое изучение Entity Framework Core", Description = "Изучение EF Core и практики использования." }
            );

            modelBuilder.Entity<Subscription>().HasData(
                new Subscription { Id = 1, UserId = 1, CourseId = 1, SubscribedOn = DateTime.SpecifyKind(new DateTime(2023, 10, 1), DateTimeKind.Utc) },
                new Subscription { Id = 2, UserId = 1, CourseId = 2, SubscribedOn = DateTime.SpecifyKind(new DateTime(2023, 10, 2), DateTimeKind.Utc) },
                new Subscription { Id = 3, UserId = 2, CourseId = 2, SubscribedOn = DateTime.SpecifyKind(new DateTime(2023, 10, 3), DateTimeKind.Utc) },
                new Subscription { Id = 4, UserId = 3, CourseId = 3, SubscribedOn = DateTime.SpecifyKind(new DateTime(2023, 10, 4), DateTimeKind.Utc) }
            );
        }
    }
}
