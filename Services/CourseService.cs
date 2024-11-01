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

        public async Task<IEnumerable<cours>> GetAllAsync()
        {
            return await _context.courses.ToListAsync();
        }

        public async Task<cours> GetByIdAsync(int id)
        {
            return await _context.courses.FindAsync(id);
        }

        public async Task<cours> CreateAsync(cours course)
        {
            // Проверка на уникальность названия курса
            if (await _context.courses.AnyAsync(c => c.Title == course.Title))
                throw new Exception("Курс с таким названием уже существует.");

            _context.courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<bool> UpdateAsync(cours course)
        {
            var existingCourse = await _context.courses.FindAsync(course.Id);
            if (existingCourse == null)
                return false;

            // Проверка на уникальность названия курса
            if (await _context.courses.AnyAsync(c => c.Title == course.Title && c.Id != course.Id))
                throw new Exception("Курс с таким названием уже существует.");

            existingCourse.Title = course.Title;
            existingCourse.Description = course.Description;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var course = await _context.courses.Include(c => c.Subscriptions).FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
                return false;

            // Удаляем связанные подписки
            if (course.Subscriptions.Any())
            {
                _context.subscriptions.RemoveRange(course.Subscriptions);
            }

            _context.courses.Remove(course);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
