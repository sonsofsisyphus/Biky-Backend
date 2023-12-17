using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Biky_Backend.ActionFilters
{
    //This class is responsible for injecting the user ID into the action method's parameters.
    public class UserIdInjectionFilter : IActionFilter
    {
        // This method is called before the action method is executed.
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var attribute = (context.ActionDescriptor as ControllerActionDescriptor)
                .MethodInfo.GetCustomAttributes(typeof(InjectUserIdAttribute), false)
                .FirstOrDefault() as InjectUserIdAttribute;

            if (attribute != null)
            {
                // Retrieve the user ID from the current user's claims.
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
