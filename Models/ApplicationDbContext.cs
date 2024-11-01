using Microsoft.EntityFrameworkCore;

namespace OnlineCoursesSubscription.Models
{
    public class ApplicationDbContext : DbContext
    {
        // DbSet
        public DbSet<users> users { get; set; }
        public DbSet<cours> courses { get; set; }
        public DbSet<subscription> subscriptions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Настройка моделей и начальные данные
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Set table name to lowercase
                entity.SetTableName(entity.GetTableName().ToLower());

                // Set column names to lowercase
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToLower());
                }

                // Set key names to lowercase
                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName().ToLower());
                }

                // Set foreign key names to lowercase
                foreach (var foreignKey in entity.GetForeignKeys())
                {
                    foreignKey.SetConstraintName(foreignKey.GetConstraintName().ToLower());
                }

                // Set index names to lowercase
                foreach (var index in entity.GetIndexes())
                {
                    index.SetDatabaseName(index.GetDatabaseName().ToLower());
                }
            }


            // Настройка первичных ключей и связей
            modelBuilder.Entity<subscription>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<subscription>()
                .HasOne(s => s.User)
                .WithMany(u => u.Subscriptions)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<subscription>()
                .HasOne(s => s.Course)
                .WithMany(c => c.Subscriptions)
                .HasForeignKey(s => s.CourseId);

            // Настройка свойств
            modelBuilder.Entity<users>()
                .Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<users>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<cours>()
                .Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<cours>()
                .Property(c => c.Description)
                .HasMaxLength(1000);

            modelBuilder.Entity<subscription>()
                .Property(s => s.SubscribedOn)
                .IsRequired();

            // Начальное заполнение данных
            modelBuilder.Entity<users>().HasData(
                new users { Id = 1, Name = "Алиса Иванова", Email = "alice@example.com" },
                new users { Id = 2, Name = "Борис Смирнов", Email = "boris@example.com" },
                new users { Id = 3, Name = "Виктор Петров", Email = "victor@example.com" }
            );

            modelBuilder.Entity<cours>().HasData(
                new cours { Id = 1, Title = "Введение в C#", Description = "Основы программирования на C#." },
                new cours { Id = 2, Title = "Продвинутый ASP.NET Core", Description = "Глубокое погружение в ASP.NET Core." },
                new cours { Id = 3, Title = "Глубокое изучение Entity Framework Core", Description = "Изучение EF Core и практики использования." }
            );

            modelBuilder.Entity<subscription>().HasData(
                new subscription { Id = 1, UserId = 1, CourseId = 1, SubscribedOn = DateTime.SpecifyKind(new DateTime(2023, 10, 1), DateTimeKind.Utc) },
                new subscription { Id = 2, UserId = 1, CourseId = 2, SubscribedOn = DateTime.SpecifyKind(new DateTime(2023, 10, 2), DateTimeKind.Utc) },
                new subscription { Id = 3, UserId = 2, CourseId = 2, SubscribedOn = DateTime.SpecifyKind(new DateTime(2023, 10, 3), DateTimeKind.Utc) },
                new subscription { Id = 4, UserId = 3, CourseId = 3, SubscribedOn = DateTime.SpecifyKind(new DateTime(2023, 10, 4), DateTimeKind.Utc) }
            );
        }
    }
}
