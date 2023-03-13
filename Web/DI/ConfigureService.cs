using Infrastructure.HttpClientService;
using Model.AppConfig;

namespace Web.DI
{
    public static class ConfigureService
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddHttpClient<WebCacheServiceClient>();
            services.AddScoped<IWebCacheServiceClient, WebCacheServiceClient>();

            return services;
        }

        public static IServiceCollection AddConfiguration(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<WebCacheServiceConfiguration>(configuration.GetSection("WebCacheServiceConfiguration"));

            return services;
        }
    }
}
