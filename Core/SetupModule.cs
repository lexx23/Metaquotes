using Common.Model;
using Common.Services;
using Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Infrastructure;

namespace Services
{
    public static class SetupModule
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            var cacheOptions = configuration.GetSection("Cache").Get<CacheOptions>();
            
            services.AddSingleton<ICacheService<Location[],string>>( (sp) => new CacheService<Location[],string>(cacheOptions));
            services.AddScoped<ILocationService, LocationService>();
            
            services.AddSingleton<ICacheService<IpLocation,string>>( (sp) => new CacheService<IpLocation,string>(cacheOptions));
            services.AddScoped<IIpRangeService, IpRangeService>();

            return services;
        }
    }
}