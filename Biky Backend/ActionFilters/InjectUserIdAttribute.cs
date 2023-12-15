namespace Biky_Backend.ActionFilters
{
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
