using FlatMate.Common.Repository;
using FlatMate.Module.Home.Models;
using FlatMate.Module.Home.Repository;
using FlatMate.Module.Home.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using prayzzz.Common.Mapping;

namespace FlatMate.Module.Home
{
    public static class Module
    {
        public static void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IDboMapper, DashboardEntryMapper>();

            services.AddSingleton<IRepository<DashboardEntryDbo>, DashboardEntryRepository>();
        }
    }
}
