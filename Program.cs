using Microsoft.EntityFrameworkCore;
using Appointment_Scheduler.Data;
using Appointment_Scheduler.Areas.Identity.Data;

namespace Appointment_Scheduler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
                        var connectionString = builder.Configuration.GetConnectionString("Appointment_SchedulerContextConnection") ?? throw new InvalidOperationException("Connection string 'Appointment_SchedulerContextConnection' not found.");

                                    builder.Services.AddDbContext<Appointment_SchedulerContext>(options =>
                options.UseSqlServer(connectionString));

                                                builder.Services.AddDefaultIdentity<Appointment_SchedulerUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<Appointment_SchedulerContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // To enable the razor pages of login/register
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            
            
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}