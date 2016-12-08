using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FlatMate.Web.Common;
using FlatMate.Web.Common.Extension;
using FlatMate.Web.Common.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using prayzzz.Common.Mapping;
using Serilog;

namespace FlatMate.Web
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            _env = env;
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                                                    .AddJsonFile("appsettings.json", true, true)
                                                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration().ReadFrom
                                                  .Configuration(Configuration)
                                                  .CreateLogger();
        }

        public IConfigurationRoot Configuration { get; }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();

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
                routes.MapRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("default", "{area=Home}/{controller=Dashboard}/{action=Index}");
            });
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            if (_env.IsDevelopment())
            {
                services.AddSwaggerGen();
            }

            services.AddMvc(o => o.Filters.Add(typeof(prayzzz.Common.Mvc.Filters.ValidationFilter)))
                    .AddControllersAsServices()
                    .AddJsonOptions(o => { o.SerializerSettings.ContractResolver = FlatMateContractResolver.Instance; });

            services.Configure<RazorViewEngineOptions>(options => { options.ViewLocationExpanders.Add(new RazorViewLocationExpander()); });

            services.AddMemoryCache();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            Module.Account.Module.ConfigureServices(services, Configuration);
            Module.Home.Module.ConfigureServices(services, Configuration);
            Module.Lists.Module.ConfigureServices(services, Configuration);

            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.InjectDependencies(typeof(Startup));
            builder.RegisterType<Mapper>().As<IMapper>().As<IMapperConfiguration>().SingleInstance();

            builder.InjectDependencies(typeof(Module.Account.Module));
            builder.InjectDependencies(typeof(Module.Home.Module));
            builder.InjectDependencies(typeof(Module.Lists.Module));

            return new AutofacServiceProvider(builder.Build());
        }
    }
}