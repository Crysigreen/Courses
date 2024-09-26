using Microsoft.EntityFrameworkCore;
using OnlineCoursesSubscription.Models;

namespace Courses.Services
{
    public class CourseService : ICourseService
    {
        private readonly ApplicationDbContext _context;

        public CourseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            return await _context.Courses.FindAsync(id);
        }

        public async Task<Course> CreateAsync(Course course)
        {
            // Проверка на уникальность названия курса
            if (await _context.Courses.AnyAsync(c => c.Title == course.Title))
                throw new Exception("Курс с таким названием уже существует.");

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<bool> UpdateAsync(Course course)
        {
            var existingCourse = await _context.Courses.FindAsync(course.Id);
            if (existingCourse == null)
                return false;

            // Проверка на уникальность названия курса
            if (await _context.Courses.AnyAsync(c => c.Title == course.Title && c.Id != course.Id))
                throw new Exception("Курс с таким названием уже существует.");

            existingCourse.Title = course.Title;
            existingCourse.Description = course.Description;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await _context.Courses.Include(c => c.Subscriptions).FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
                return false;

            // Удаляем связанные подписки
            if (course.Subscriptions.Any())
            {
                _context.Subscriptions.RemoveRange(course.Subscriptions);
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
