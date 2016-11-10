using FlatMate.Common.Repository;
using FlatMate.Module.Lists.Models;
using FlatMate.Module.Lists.Repository;
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
            services.AddScoped<IRepository<ItemListDbo>, ItemListRepository>();

            services.AddScoped<IItemListService, ItemListService>();

            services.AddScoped<IDboMapper, ItemMapper>();
            services.AddScoped<IDboMapper, ItemListMapper>();
            services.AddScoped<IDboMapper, ItemListGroupMapper>();

            services.AddSingleton<ItemListPrivileger, ItemListPrivileger>();

            services.AddDbContext<ListsContext>(options => options.UseMySQL(configuration.GetConnectionString("DefaultConnection")));
        }

        public static void Configure(IApplicationBuilder app)
        {
        }
    }
}