using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FlatMate.Web.Common.Filter;
using FlatMate.Web.Common.Json;
using FlatMate.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using prayzzz.Common.Mapping;

namespace FlatMate.Web
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            
            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Module.Account.Module.ConfigureServices(services, Configuration);
            Module.Lists.Module.ConfigureServices(services, Configuration);

            services.AddMvc().AddControllersAsServices()
                             .AddJsonOptions(o => { o.SerializerSettings.ContractResolver = FlatMateContractResolver.Instance; });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IJsonService, JsonService>();

            services.AddSingleton<IRequestResultService, RequestResultService>();
            services.AddSingleton<ApiResultFilter, ApiResultFilter>();
            services.AddSingleton<MvcResultFilter, MvcResultFilter>();

            if (_env.IsDevelopment())
            {
                services.AddSwaggerGen();
            }

            var builder = new ContainerBuilder();
            builder.RegisterType<Mapper>().As<IMapper>().As<IMapperConfiguration>().SingleInstance();
            builder.Populate(services);

            var container = builder.Build();
            return container.Resolve<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                app.UseSwagger();
                app.UseSwaggerUi();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            Module.Account.Module.Configure(app);

            app.UseMvc(routes =>
            {
                routes.MapRoute("areaRoute",
                    "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    "default",
                    "{controller=Dashboard}/{action=Index}/{id?}");
            });
        }
    }
}
