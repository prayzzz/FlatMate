using FlatMate.Module.Account.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySQL.Data.EntityFrameworkCore.Extensions;

namespace FlatMate.Module.Account
{
    public static class Module
    {
        public static void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddSingleton<IPasswordService, PasswordService>();

            services.AddDbContext<AccountContext>(options => options.UseMySQL(configuration.GetConnectionString("DefaultConnection")));
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = "FlatMate",
                LoginPath = new PathString("/Account/Login/"),
                AccessDeniedPath = new PathString("/Account/Login/"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                ClaimsIssuer = "FlatMate",
                SlidingExpiration = true
            });
        }
    }
}