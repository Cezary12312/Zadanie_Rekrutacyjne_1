using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Zadanie_Rekrutacyjne_1.Data.DataAccess;
using Zadanie_Rekrutacyjne_1.Data.Repository;
using Zadanie_Rekrutacyjne_1.Models;
using Zadanie_Rekrutacyjne_1.Utilities.DbInitilizer;
using Zadanie_Rekrutacyjne_1.Utilities.Validators;

namespace Zadanie_Rekrutacyjne_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<UserDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IDbInit, DbInit>();
            builder.Services.AddScoped<IValidator<User>, DateOfBirthValidator>();

            builder.Services.AddAntiforgery(options =>
            {
                options.FormFieldName = "AntiforgeryFieldname";
                options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
                options.SuppressXFrameOptionsHeader = false;
            });
     
            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            using (var scope = app.Services.CreateScope())
            {
                var DbInitilizer = scope.ServiceProvider.GetRequiredService<IDbInit>();
                DbInitilizer.Initialize();
            }

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}