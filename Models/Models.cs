using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OnlineCoursesSubscription.Models
{
    public class users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<subscription> ?Subscriptions { get; set; }
    }

    public class cours
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<subscription> ?Subscriptions { get; set; }
    }

    public class subscription
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime SubscribedOn { get; set; }
        public users ?User { get; set; }
        public cours ?Course { get; set; }
    }

    public class Resource
    {
        [JsonPropertyName("_links")]
        public Dictionary<string, object> Links { get; set; } = new Dictionary<string, object>();
    }

    public class UserResource : Resource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

    }

    public class CourseResource : Resource
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class SubscriptionResource : Resource
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime SubscribedOn { get; set; }
        public DateTime Expireson { get; set; }
    }





}
