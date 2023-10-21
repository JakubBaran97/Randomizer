using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Randomizer.Authorization;
using Randomizer.Entities;
using Randomizer.Middleweare;
using Randomizer.Services;


namespace Randomizer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(s =>
                {
                    s.LoginPath = "/login";
                    s.LogoutPath= "/logout";
                });
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<RandomizerDbContext>();
            builder.Services.AddTransient<IRandomizerService, RandomizerServices>();
            builder.Services.AddTransient<IAccountServices, AccountServices>();
            builder.Services.AddTransient<IQRService, QRService>();
            builder.Services.AddTransient<IUserContextService, UserContextService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
            
            builder.Services.AddTransient<ErrorHandlingMiddleweare>();
            builder.Services.AddTransient<IAuthorizationHandler, MenuOperationRequirementHandler>();
            builder.Services.AddTransient<IAuthorizationHandler, ProductOperationRequirementHandler>();

            var app = builder.Build();


            

           

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Randomizer}/{action=Index}/{id?}");

            app.Run();
        }
    }
}