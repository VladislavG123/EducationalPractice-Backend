using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenPoint.Dotnet.Contracts.Options;
using GreenPoint.Dotnet.DataAccess;
using GreenPoint.Dotnet.DataAccess.Providers;
using GreenPoint.Dotnet.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace GreenPoint.Dotnet.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.core.json")
                .Build();
            ;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            // DbContext
            /*services.AddDbContext<ApplicationContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("DevConnection")));*/
            services.AddDbContext<ApplicationContext>(
                options => options.UseInMemoryDatabase("MEMORY_DB"));

            // Providers
            services.AddTransient<UserProvider>();
            services.AddTransient<AvatarProvider>();
            services.AddTransient<CommentProvider>();
            services.AddTransient<LikeProvider>();
            services.AddTransient<SpotProvider>();
            services.AddTransient<SpotImageProvider>();
            services.AddTransient<AdminProvider>();
            services.AddTransient<StatusProvider>();
            services.AddTransient<UserCodeProvider>();


            // Authentication
            services.Configure<SecretOption>(Configuration.GetSection("Secrets"));

            services.AddTransient<UserAuthenticationService>();
            
            // configure jwt authentication
            var secrets = Configuration.GetSection("Secrets");

            var key = Encoding.ASCII.GetBytes(secrets.GetValue<string>("JWTSecret"));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });


            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "GreenPoint.Dotnet",
                    Description = "WebApi",

                });
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = " JWT    'Bearer '  !",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }

                    }
                });
            });
            
            services.AddCors(options =>
            {
                options.AddPolicy(name: "Policy",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GreenPoint.Dotnet.WebApi v1"));
            
            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}