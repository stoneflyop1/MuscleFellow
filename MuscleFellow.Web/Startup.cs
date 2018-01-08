using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MuscleFellow.Models;
using MuscleFellow.Models.Domain;
using Autofac;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MuscleFellow.Web
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
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<MuscleFellowDbContext>(options => options.UseSqlite(connectionString));
            services.AddIdentity<ApplicationUser, IdentityRole>().
                AddEntityFrameworkStores<MuscleFellowDbContext>().AddDefaultTokenProviders();
            services.AddSession(options=>options.IdleTimeout = TimeSpan.FromMinutes(20));
            // https://docs.microsoft.com/en-us/aspnet/core/mvc/razor-pages/razor-pages-convention-features#configure-a-page-route
            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/Products/Details", "Product/{id?}");
                options.Conventions.AddPageRoute("/Index", "Home");
            });

            //https://github.com/aspnet/Security/issues/1310
            // Add JWT��Protection
            var secretKey = "mysupersecret_secretkey!123";
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match! 
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                // Validate the JWT Issuer (iss) claim 
                ValidateIssuer = true,
                ValidIssuer = "MuscleFellow",
                // Validate the JWT Audience (aud) claim 
                ValidateAudience = true,
                ValidAudience = "MuscleFellowAudience",
                // Validate the token expiry 
                ValidateLifetime = true,
                // If you want to allow a certain amount of clock drift, set that here: 
                ClockSkew = TimeSpan.Zero
            };
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
            });
            //services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
        }


        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Add any Autofac modules or registrations.
            // This is called AFTER ConfigureServices so things you
            // register here OVERRIDE things registered in ConfigureServices.
            //
            // You must have the call to AddAutofac in the Program.Main
            // method or this won't be called.
            builder.RegisterModule(new AutofacModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
            

            // https://docs.microsoft.com/en-us/ef/core/get-started/aspnetcore/new-db
            // Loading sample data.
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<MuscleFellowDbContext>();
                bool hasCreated = dbContext.Database.EnsureCreated();
                if (hasCreated)
                {
                    var dbInitializer = new MuscleFellowSampleDataInitializer(dbContext);
                    dbInitializer.LoadBasicInformationAsync().Wait();
                    dbInitializer.LoadSampleDataAsync().Wait();
                }
            }
        }
    }
}
