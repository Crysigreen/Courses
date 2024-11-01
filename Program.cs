
using Courses.GraphQL.Mutations;
using Courses.GraphQL.Queries;
using Courses.Services;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesSubscription.Data;
using OnlineCoursesSubscription.Models;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Courses
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();


            builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<ICourseStorage, CourseStorage>();
            builder.Services
                .AddGraphQLServer()
                .AddQueryType<GraphQL.Queries.Query>()
                .AddMutationType<Mutation>()
                .AddProjections()
                .AddFiltering()
                .AddSorting();

            var app = builder.Build();

            // Применение миграций и заполнение данных
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                // Применяем миграции
                context.Database.Migrate();
                // Заполняем данными
                DataSeeder.Seed(context);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGraphQL();


            app.MapControllers();

            app.Run();
        }
    }
}
