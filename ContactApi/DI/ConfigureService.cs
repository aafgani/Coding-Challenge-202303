using Infrastructure.Cache;
using Model.AppConfig;

namespace ContactApi.DI
{
    public static class ConfigureService
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<ICacheHelper<int>, MyOwnCacheHelper<int>>();

            return services;
        }

        public static IServiceCollection AddConfiguration(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<CacheConfiguration>(configuration.GetSection("CacheConfiguration"));

            return services;
        }
    }
}
