using OnlineCoursesSubscription.Models;

namespace OnlineCoursesSubscription.Extensions
{
    public static class ResourceExtensions
    {
        public static void AddSelfLink(this Resource resource, HttpContext httpContext, string actionName, string controllerName, object routeValues)
        {
            var linkGenerator = httpContext.RequestServices.GetRequiredService<LinkGenerator>();
            resource.Links["self"] = new
            {
                href = linkGenerator.GetPathByAction(httpContext, actionName, controllerName, routeValues)
            };
        }

        public static void AddLink(this Resource resource, string rel, HttpContext httpContext, string actionName, string controllerName, object routeValues)
        {
            var linkGenerator = httpContext.RequestServices.GetRequiredService<LinkGenerator>();
            resource.Links[rel] = new
            {
                href = linkGenerator.GetPathByAction(httpContext, actionName, controllerName, routeValues)
            };
        }
    }
}
