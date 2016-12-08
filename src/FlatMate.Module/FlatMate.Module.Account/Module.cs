using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlatMate.Module.Account
{
    public static class Module
    {
        public static void Configure(IApplicationBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "FlatMate",
                LoginPath = new PathString("/Account/Login/"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                ClaimsIssuer = "FlatMate",
                SlidingExpiration = true,
                Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = context =>
                    {
                        if (context.Request.Path.StartsWithSegments("/api/v1"))
                        {
                            context.Response.StatusCode = 401;
                        }
                        else
                        {
                            context.Response.Redirect(context.RedirectUri);
                        }

                        return Task.FromResult(0);
                    }
                }
            });
        }

        public static void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddDbContext<AccountContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}