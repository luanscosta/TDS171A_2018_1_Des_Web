using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoMvc.Data;
using TodoMvc.Models;
using TodoMvc.Services;

namespace TodoMvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options=> {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                //googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                //googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                googleOptions.ClientId = "924422722083-i66j3pr9o56hkei9kjkis15t32jgpv18.apps.googleusercontent.com";
                googleOptions.ClientSecret = "hzNji9DXbcnEXXHj-ldwVn4a";
            });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddScoped<ITodoItemService, TodoItemService>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
                            IHostingEnvironment env,
                            UserManager<ApplicationUser> userManager,
                            RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            EnsureRoleAsync(roleManager).Wait();
            EnsureTestAdminAsync(userManager).Wait();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static async Task EnsureRoleAsync(RoleManager<IdentityRole> roleManager)
        {
            var adminAlreadyExists = await roleManager.RoleExistsAsync(Constants.AdministratorRole);
            var userAlreadyExists = await roleManager.RoleExistsAsync(Constants.UserRole);

            if (!adminAlreadyExists)
                await roleManager.CreateAsync(new IdentityRole(Constants.AdministratorRole));
            if (!userAlreadyExists)
                await roleManager.CreateAsync(new IdentityRole(Constants.UserRole));
        }

        private static async Task EnsureTestAdminAsync(UserManager<ApplicationUser> userManager)
        {
            var testAdmin = await userManager.Users
                .Where(x => x.UserName == "admin@todo.local")
                .SingleOrDefaultAsync();

            if (testAdmin != null)
                return;

            testAdmin = new ApplicationUser
            {
                UserName = "admin@todo.local",
                Email = "admin@todo.local"
            };

            await userManager.CreateAsync(testAdmin, "NotSecure123!!");

            await userManager.AddToRoleAsync(testAdmin, Constants.AdministratorRole);
        }
    }
}
