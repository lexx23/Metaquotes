using System.Threading.Tasks;
using Common.DataProviders;
using Common.Settings;
using DAL.Binary.DataProviders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.Binary
{
    public static class SetupModule
    {
        public static async Task<IServiceCollection> AddBinaryDataBase(this IServiceCollection services, IConfiguration configuration)
        {
            var dbOptions = configuration.GetSection("Database").Get<DatabaseOptions>();
            var context = new DatabaseContext(dbOptions);
            await context.Initialize();
            
            services.AddSingleton(context);
            services.AddScoped<ILocationDataProvider, LocationDataProvider>();
            services.AddScoped<IIpRangeDataProvider, IpRangeDataProvider>();

            return services;
        }
    }
}