using Courses.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesSubscription.Extensions;
using OnlineCoursesSubscription.Models;

namespace OnlineCoursesSubscription.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly LinkGenerator _linkGenerator;

        public UsersController(IUserService userService, ISubscriptionService subscriptionService, LinkGenerator linkGenerator)
        {
            _userService = userService;
            _subscriptionService = subscriptionService;
            _linkGenerator = linkGenerator;
        }

        // GET: api/users
        [HttpGet(Name = nameof(GetUsers))]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllAsync();

            var resources = users.Select(u =>
            {
                var resource = new UserResource
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email
                };
                resource.AddSelfLink(HttpContext, nameof(GetUser), "Users", new { id = u.Id });
                resource.AddLink("subscriptions", HttpContext, nameof(GetUserSubscriptions), "Users", new { userId = u.Id });
                return resource;
            }).ToList();

            var collectionResource = new
            {
                users = resources,
                _links = new Dictionary<string, object>
                {
                    ["self"] = new
                    {
                        href = _linkGenerator.GetPathByAction(HttpContext, nameof(GetUsers), "Users", null)
                    }
                }
            };

            return Ok(collectionResource);
        }

        // GET: api/users/5
        [HttpGet("{id}", Name = nameof(GetUser))]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            var resource = new UserResource
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };

            resource.AddSelfLink(HttpContext, nameof(GetUser), "Users", new { id = user.Id });
            resource.AddLink("subscriptions", HttpContext, nameof(GetUserSubscriptions), "Users", new { userId = user.Id });

            return Ok(resource);
        }

        // POST: api/users
        [HttpPost(Name = nameof(CreateUser))]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdUser = await _userService.CreateAsync(user);

                var resource = new UserResource
                {
                    Id = createdUser.Id,
                    Name = createdUser.Name,
                    Email = createdUser.Email
                };

                resource.AddSelfLink(HttpContext, nameof(GetUser), "Users", new { id = createdUser.Id });
                resource.AddLink("subscriptions", HttpContext, nameof(GetUserSubscriptions), "Users", new { userId = createdUser.Id });

                return CreatedAtRoute(nameof(GetUser), new { id = createdUser.Id }, resource);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT: api/users/5
        [HttpPut("{id}", Name = nameof(UpdateUser))]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != user.Id)
                return BadRequest("Идентификатор пользователя не совпадает.");

            try
            {
                var result = await _userService.UpdateAsync(user);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE: api/users/5
        [HttpDelete("{id}", Name = nameof(DeleteUser))]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        // GET: api/users/5/subscriptions
        [HttpGet("{userId}/subscriptions", Name = nameof(GetUserSubscriptions))]
        public async Task<IActionResult> GetUserSubscriptions(int userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
                return NotFound();

            var subscriptions = await _subscriptionService.GetByUserIdAsync(userId);

            var resources = subscriptions.Select(s =>
            {
                var resource = new SubscriptionResource
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    CourseId = s.CourseId,
                    SubscribedOn = s.SubscribedOn
                };
                resource.AddSelfLink(HttpContext, nameof(SubscriptionsController.GetSubscription), "Subscriptions", new { id = s.Id });
                resource.AddLink("course", HttpContext, nameof(CoursesController.GetCourse), "Courses", new { id = s.CourseId });
                return resource;
            }).ToList();

            var collectionResource = new
            {
                subscriptions = resources,
                _links = new Dictionary<string, object>
                {
                    ["self"] = new
                    {
                        href = _linkGenerator.GetPathByAction(HttpContext, nameof(GetUserSubscriptions), "Users", new { userId })
                    },
                    ["user"] = new
                    {
                        href = _linkGenerator.GetPathByAction(HttpContext, nameof(GetUser), "Users", new { id = userId })
                    }
                }
            };

            return Ok(collectionResource);
        }
    }
}
