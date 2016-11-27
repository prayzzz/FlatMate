using FlatMate.Common.Provider;
using FlatMate.Common.Repository;
using FlatMate.Module.Lists.Models;
using FlatMate.Module.Lists.Provider;
using FlatMate.Module.Lists.Repository;
using FlatMate.Module.Lists.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using prayzzz.Common.Mapping;

namespace FlatMate.Module.Lists
{
    public static class Module
    {
        public static void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddScoped<IRepository<ItemListDbo>, ItemListRepository>();

            services.AddScoped<IItemListService, ItemListService>();

            services.AddScoped<IDboMapper, ItemMapper>();
            services.AddScoped<IDboMapper, ItemListMapper>();
            services.AddScoped<IDboMapper, ItemListGroupMapper>();

            services.AddSingleton<ItemListPrivileger, ItemListPrivileger>();
            services.AddSingleton<IDashboardEntryProvider, DashboardEntryProvider>();

            services.AddDbContext<ListsContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

        public static void Configure(IApplicationBuilder app)
        {
        }
    }
}