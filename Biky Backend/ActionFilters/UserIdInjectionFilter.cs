using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Biky_Backend.ActionFilters
{
    public class UserIdInjectionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var attribute = (context.ActionDescriptor as ControllerActionDescriptor)
                .MethodInfo.GetCustomAttributes(typeof(InjectUserIdAttribute), false)
                .FirstOrDefault() as InjectUserIdAttribute;

            if (attribute != null)
            {
                var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return;

                foreach (var arg in context.ActionArguments)
                {
                    if (attribute.TargetType.IsInstanceOfType(arg.Value))
                    {
                        var property = attribute.TargetType.GetProperty(attribute.UserIdPropertyName);
                        if (property != null && property.PropertyType == typeof(Guid))
                        {
                            property.SetValue(arg.Value, new Guid(userId));
                        }
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }

}
