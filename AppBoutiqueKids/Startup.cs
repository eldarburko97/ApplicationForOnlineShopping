using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppBoutiqueKids.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AppBoutiqueKids.Models;
using AppBoutiqueKids.Services;
using AppBoutiqueKids.Repository;
using AppBoutiqueKids.Helpers;
using Microsoft.AspNetCore.SignalR;
using AppBoutiqueKids.Hubs;

namespace AppBoutiqueKids
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
           
            services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader()
                       .WithOrigins("https://localhost:44302")
                       .AllowCredentials();
            }));
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            

            services.AddIdentity<User, Role>(config => {
                config.SignIn.RequireConfirmedEmail = false;
                config.Password.RequireDigit = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultUI()
              .AddDefaultTokenProviders();
            
            services.AddScoped<IUserData, UserData>();
            services.AddScoped<IProduct, ProductRepository>();
            services.AddScoped<ISupplier, SupplierRepository>();
            services.AddScoped<ICategory, CategoryRepository>();
            services.AddScoped<ISize, SizeRepository>();
            services.AddScoped<IBrand, BrandRepository>();
            services.AddScoped<IShipper, ShipperRepository>();
            services.AddScoped<IProductSize, ProductSizeRepository>();
            services.AddScoped<IOrderDetails, OrderDetailsRepository>();
            services.AddScoped<ICartDetails, CartDetailsRepository>();
            services.AddScoped<IOrder, OrderRepostitory>();
            services.AddSignalR();
            services.AddCloudscribePagination();
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
       
            app.UseAuthentication();

            app.UseSignalR(config =>
            {
                config.MapHub<DeliverHub>("/deliverHub");
                config.MapHub<NotificationsHub>("/cart");
            });

            app.UseMvc(routes =>
            {
                
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            //CreateUserRoles(serviceProvider).Wait();
        }
        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();

            string[] roleNames = { "Admin", "Member" };

            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new Role(roleName));
                }
            }

            var poweruser = new User
            {
                UserName = Configuration["UserSettings:Username"],
                Email = Configuration["UserSettings:UserEmail"]
            };

            string userPWD = Configuration["UserSettings:UserPassword"];
            var _user = await UserManager.FindByEmailAsync(Configuration["UserSettings:AdminUserEmail"]);

            if (_user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
                if (createPowerUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(poweruser, roleNames[1]);
                    await UserManager.AddToRoleAsync(poweruser, roleNames[2]);
                }
            }
        }
    }
}
