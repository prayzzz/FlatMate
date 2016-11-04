using FlatMate.Module.Lists.Models;
using FlatMate.Module.Lists.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySQL.Data.EntityFrameworkCore.Extensions;
using prayzzz.Common.Mapping;

namespace FlatMate.Module.Lists
{
    public static class Module
    {
        public static void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddScoped<IListService, ListService>();

            services.AddScoped<IDboMapper, ItemMapper>();
            services.AddScoped<IDboMapper, ItemListMapper>();
            services.AddScoped<IDboMapper, ItemListGroupMapper>();

            services.AddDbContext<ListsContext>(options => options.UseMySQL(configuration.GetConnectionString("DefaultConnection")));
        }

        public static void Configure(IApplicationBuilder app)
        {
        }
    }
}