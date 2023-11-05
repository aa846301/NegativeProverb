using Microsoft.Extensions.DependencyInjection;


namespace Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ServiceAttribute : Attribute
    {
        public ServiceLifetime Lifetime { get; set; }


        public ServiceAttribute(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            Lifetime = serviceLifetime;
        }
    }
}
