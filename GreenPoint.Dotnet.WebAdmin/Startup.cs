using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenPoint.Dotnet.DataAccess;
using GreenPoint.Dotnet.DataAccess.Providers;
using GreenPoint.Dotnet.WebAdmin.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GreenPoint.Dotnet.WebAdmin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.core.json")
                .Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Options
            services.Configure<AwsS3Options>(Configuration.GetSection("AwsS3"));

            // DbContext
            services.AddDbContext<ApplicationContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("DevConnection")));

            services.AddTransient<AdminAuthenticationService>();
            services.AddTransient<AdminProvider>();
            services.AddTransient<SpotProvider>();
            services.AddTransient<AvatarProvider>();
            services.AddTransient<StatusProvider>();
            services.AddTransient<UserProvider>();
            services.AddTransient<SpotImageProvider>();
            services.AddTransient<CommentProvider>();
                            
            services.AddScoped<AwsS3FileUploadService>();
            
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth";
                });

            
            services.AddRazorPages();
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }
    }
}