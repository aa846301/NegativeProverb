using Microsoft.Extensions.DependencyInjection;


namespace Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ServiceAttribute : Attribute
    {

        public ServiceLifetime Lifetime { get; set; }
        public Type InterfaceType { get; set; }

        public ServiceAttribute(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            Lifetime = serviceLifetime;
        }
        public ServiceAttribute(Type interfaceType, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            InterfaceType = interfaceType;
            Lifetime = serviceLifetime;
        }
    }
}
