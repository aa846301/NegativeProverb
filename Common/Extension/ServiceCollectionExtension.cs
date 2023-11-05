using Common.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System.Reflection;


namespace Common.Extension
{
    public static class ServiceCollectionExtension
    {

        public static void AddBusiness(this IServiceCollection services)
        {
            var assemblies = DependencyContext.Default.RuntimeLibraries
                .SelectMany(lib => lib.GetDefaultAssemblyNames(DependencyContext.Default))
                .Select(Assembly.Load)
                .ToList();

            var typesWithAttribute = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && !type.IsAbstract && type.GetCustomAttribute<ServiceAttribute>() != null);

            foreach (var type in typesWithAttribute)
            {
                var attribute = type.GetCustomAttribute<ServiceAttribute>();
                var lifetime = attribute.Lifetime;

                switch (lifetime)
                {
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(type);
                        break;
                    case ServiceLifetime.Transient:
                        services.AddTransient(type);
                        break;
                    case ServiceLifetime.Scoped:
                        services.AddScoped(type);
                        break;
                }
            }
        }

    }
}
