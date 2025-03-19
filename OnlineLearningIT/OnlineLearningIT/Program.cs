using Microsoft.EntityFrameworkCore;
using OnlineLearningIT.Models;
using OnlineLearningIT.Repositories.Implementations;
using OnlineLearningIT.Repositories.Interfaces;
using OnlineLearningIT.Services.Implementations;
using OnlineLearningIT.Services.Interfaces;

namespace OnlineLearningIT
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<OnlineLearningItContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineLearningIT"),
                sqlOptions => sqlOptions.EnableRetryOnFailure())
            );
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<ICertificateRepository, CertificateRepository>();
            builder.Services.AddScoped<ICertificateService, CertificateService>();
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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
