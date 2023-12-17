namespace Biky_Backend.ActionFilters
{
    // This custom attribute is used to inject the user ID into a controller action method.
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class InjectUserIdAttribute : Attribute
    {
        public Type TargetType { get; private set; }
        public string UserIdPropertyName { get; private set; }

        public InjectUserIdAttribute(Type targetType, string userIdPropertyName)
        {
            TargetType = targetType;
            UserIdPropertyName = userIdPropertyName;
        }
    }

}
