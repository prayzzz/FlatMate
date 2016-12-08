using FlatMate.Module.Lists.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlatMate.Module.Lists
{
    public static class Module
    {
        public static void Configure(IApplicationBuilder app)
        {
        }

        public static void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddDbContext<ListsContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}