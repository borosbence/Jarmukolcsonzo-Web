using Jarmukolcsonzo.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Jarmukolcsonzo.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            string? connectionString = builder.Configuration.GetConnectionString("JKDB");
            builder.Services.AddDbContext<JKContext>(options =>
                           options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
