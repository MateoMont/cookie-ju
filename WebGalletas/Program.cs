using Dominio.Data; // <-- Asegurate de tener este using
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
namespace WebGalletas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ?? Agregamos AppDbContext al contenedor de servicios
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Servicios MVC
            builder.Services.AddControllersWithViews();

            builder.Services.AddSession();
            var app = builder.Build();

            // Middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");



            app.UseSession();

            app.Run();
        }
    }
}
