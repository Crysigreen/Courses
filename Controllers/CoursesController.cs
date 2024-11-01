using Courses.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineCoursesSubscription.Extensions;
using OnlineCoursesSubscription.Models;

namespace OnlineCoursesSubscription.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly LinkGenerator _linkGenerator;

        public CoursesController(ICourseService courseService, LinkGenerator linkGenerator)
        {
            _courseService = courseService;
            _linkGenerator = linkGenerator;
        }

        // GET: api/courses
        [HttpGet(Name = nameof(GetCourses))]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _courseService.GetAllAsync();

            var resources = courses.Select(c =>
            {
                var resource = new CourseResource
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description
                };
                resource.AddSelfLink(HttpContext, nameof(GetCourse), "Courses", new { id = c.Id });
                resource.Links["subscribe"] = new
                {
                    href = _linkGenerator.GetPathByAction(HttpContext, nameof(SubscriptionsController.CreateSubscription), "Subscriptions", null),
                    method = "POST"
                };
                return resource;
            }).ToList();

            var collectionResource = new
            {
                courses = resources,
                _links = new Dictionary<string, object>
                {
                    ["self"] = new
                    {
                        href = _linkGenerator.GetPathByAction(HttpContext, nameof(GetCourses), "Courses", null)
                    }
                }
            };

            return Ok(collectionResource);
        }

        // GET: api/courses/5
        [HttpGet("{id}", Name = nameof(GetCourse))]
        public async Task<IActionResult> GetCourse(int id)
        {
            var course = await _courseService.GetByIdAsync(id);

            if (course == null)
                return NotFound();

            var resource = new CourseResource
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description
            };

            resource.AddSelfLink(HttpContext, nameof(GetCourse), "Courses", new { id = course.Id });
            resource.Links["subscribe"] = new
            {
                href = _linkGenerator.GetPathByAction(HttpContext, nameof(SubscriptionsController.CreateSubscription), "Subscriptions", null),
                method = "POST"
            };

            return Ok(resource);
        }

        // POST: api/courses
        [HttpPost(Name = nameof(CreateCourse))]
        public async Task<IActionResult> CreateCourse([FromBody] cours course)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdCourse = await _courseService.CreateAsync(course);

                var resource = new CourseResource
                {
                    Id = createdCourse.Id,
                    Title = createdCourse.Title,
                    Description = createdCourse.Description
                };

                resource.AddSelfLink(HttpContext, nameof(GetCourse), "Courses", new { id = createdCourse.Id });
                resource.Links["subscribe"] = new
                {
                    href = _linkGenerator.GetPathByAction(HttpContext, nameof(SubscriptionsController.CreateSubscription), "Subscriptions", null),
                    method = "POST"
                };

                return CreatedAtRoute(nameof(GetCourse), new { id = createdCourse.Id }, resource);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT: api/courses/5
        [HttpPut("{id}", Name = nameof(UpdateCourse))]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] cours course)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != course.Id)
                return BadRequest("Идентификатор курса не совпадает.");

            try
            {
                var result = await _courseService.UpdateAsync(course);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE: api/courses/5
        [HttpDelete("{id}", Name = nameof(DeleteCourse))]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var result = await _courseService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
