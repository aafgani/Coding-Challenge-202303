using Infrastructure.Cache;

namespace ContactApi.DI
{
    public static class ConfigureService
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<ICacheHelper<int>, CacheHelper<int>>();

            return services;
        }
    }
}
