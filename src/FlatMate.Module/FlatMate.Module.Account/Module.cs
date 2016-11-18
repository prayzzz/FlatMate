using System.Threading.Tasks;
using FlatMate.Common.Repository;
using FlatMate.Module.Account.Models;
using FlatMate.Module.Account.Repository;
using FlatMate.Module.Account.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using prayzzz.Common.Mapping;

namespace FlatMate.Module.Account
{
    public static class Module
    {
        public static void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddScoped<IRepository<UserDbo>, UserRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddSingleton<IPasswordService, PasswordService>();

            services.AddScoped<IDboMapper, UserMapper>();

            services.AddDbContext<AccountContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

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
    }
}