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
            builder.Services.AddScoped<IRandomizerService, RandomizerServices>();
            builder.Services.AddScoped<IAccountServices, AccountServices>();
            builder.Services.AddScoped<IQRService, QRService>();
            builder.Services.AddScoped<IUserContextService, UserContextService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            
            builder.Services.AddScoped<ErrorHandlingMiddleweare>();
            builder.Services.AddScoped<IAuthorizationHandler, MenuOperationRequirementHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, ProductOperationRequirementHandler>();

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