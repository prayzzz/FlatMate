using FlatMate.Web.Areas.Home.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using prayzzz.Common.Mapping;

namespace FlatMate.Web.Areas.Home
{
    public static class HomeArea
    {
        public static void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddScoped<IDboMapper, DashboardEntryMapper>();
        }
    }
}