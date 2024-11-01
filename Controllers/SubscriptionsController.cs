using Courses.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OnlineCoursesSubscription.Extensions;
using OnlineCoursesSubscription.Models;
using System;
using System.Security.Claims;

namespace OnlineCoursesSubscription.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly LinkGenerator _linkGenerator;

        public SubscriptionsController(ISubscriptionService subscriptionService, LinkGenerator linkGenerator)
        {
            _subscriptionService = subscriptionService;
            _linkGenerator = linkGenerator;
        }

        // GET: api/subscriptions
        [HttpGet(Name = nameof(GetSubscriptions))]
        public async Task<IActionResult> GetSubscriptions()
        {
            var subscriptions = await _subscriptionService.GetAllAsync();

            var resources = subscriptions.Select(s =>
            {
                var resource = new SubscriptionResource
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    CourseId = s.CourseId,
                    SubscribedOn = s.SubscribedOn
                };
                resource.AddSelfLink(HttpContext, nameof(GetSubscription), "Subscriptions", new { id = s.Id });
                resource.AddLink("user", HttpContext, nameof(UsersController.GetUser), "Users", new { id = s.UserId });
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
                        href = _linkGenerator.GetPathByAction(HttpContext, nameof(GetSubscriptions), "Subscriptions", null)
                    }
                }
            };

            return Ok(collectionResource);
        }

        // GET: api/subscriptions/5
        [HttpGet("{id}", Name = nameof(GetSubscription))]
        public async Task<IActionResult> GetSubscription(int id)
        {
            var subscription = await _subscriptionService.GetByIdAsync(id);

            if (subscription == null)
                return NotFound();

            var resource = new SubscriptionResource
            {
                Id = subscription.Id,
                UserId = subscription.UserId,
                CourseId = subscription.CourseId,
                SubscribedOn = subscription.SubscribedOn
            };

            resource.AddSelfLink(HttpContext, nameof(GetSubscription), "Subscriptions", new { id = subscription.Id });
            resource.AddLink("user", HttpContext, nameof(UsersController.GetUser), "Users", new { id = subscription.UserId });
            resource.AddLink("course", HttpContext, nameof(CoursesController.GetCourse), "Courses", new { id = subscription.CourseId });

            return Ok(resource);
        }

        // POST: api/subscriptions
        [HttpPost(Name = nameof(CreateSubscription))]
        public async Task<IActionResult> CreateSubscription([FromBody] subscription subscription)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdSubscription = await _subscriptionService.CreateAsync(subscription);

                var resource = new SubscriptionResource
                {
                    Id = createdSubscription.Id,
                    UserId = createdSubscription.UserId,
                    CourseId = createdSubscription.CourseId,
                    SubscribedOn = createdSubscription.SubscribedOn
                };

                resource.AddSelfLink(HttpContext, nameof(GetSubscription), "Subscriptions", new { id = createdSubscription.Id });
                resource.AddLink("user", HttpContext, nameof(UsersController.GetUser), "Users", new { id = createdSubscription.UserId });
                resource.AddLink("course", HttpContext, nameof(CoursesController.GetCourse), "Courses", new { id = createdSubscription.CourseId });

                return CreatedAtRoute(nameof(GetSubscription), new { id = createdSubscription.Id }, resource);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT: api/subscriptions/5
        [HttpPut("{id}", Name = nameof(UpdateSubscription))]
        public async Task<IActionResult> UpdateSubscription(int id, [FromBody] subscription subscription)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != subscription.Id)
                return BadRequest("Идентификатор подписки не совпадает.");

            try
            {
                var result = await _subscriptionService.UpdateAsync(subscription);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // DELETE: api/subscriptions/5
        [HttpDelete("{id}", Name = nameof(DeleteSubscription))]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            var result = await _subscriptionService.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

}
