using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MuscleFellow.Models;
using MuscleFellow.Models.Domain;
using System;
using System.IO;
using static System.Console;

namespace MuscleFellow.Console
{
    class Program
    {
        //https://stackoverflow.com/questions/38114761/asp-net-core-configuration-for-net-core-console-application
        private static IConfigurationRoot Configuration;

        private static ILoggerFactory LoggerFactory;

        static void Main(string[] args)
        {
            SetupConfig();
            var serviceProvider = ConfigServices();
            SampleData(serviceProvider);
            WriteLine("Hello World!");
            WriteLine("Press Any Key to Continue...");
            ReadKey();
        }

        static IConfigurationRoot SetupConfig()
        {
            var baseFolder = AppContext.BaseDirectory;  //Directory.GetCurrentDirectory();  //
            var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(baseFolder))
            .AddJsonFile("appsettings.json", optional: true);

            Configuration = builder.Build();

            return Configuration;
        }

        static IServiceProvider ConfigServices()
        {
            LoggerFactory = new LoggerFactory()
                .AddConsole(Configuration.GetSection("Logging"));//.AddDebug();

            var services = new ServiceCollection();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<MuscleFellowDbContext>(options => options.UseSqlite(connectionString));
            services.AddIdentity<ApplicationUser, IdentityRole>().
                AddEntityFrameworkStores<MuscleFellowDbContext>().AddDefaultTokenProviders();

            return services.BuildServiceProvider();
        }

        static void SampleData(IServiceProvider serviceProvider)
        {
            var log = LoggerFactory.CreateLogger("data_logger");
            log.LogInformation("Start write data...");
            using (var dbContext = serviceProvider.GetRequiredService<MuscleFellowDbContext>())
            {
                dbContext.Database.EnsureCreated();
                var dbInitializer = new MuscleFellowSampleDataInitializer(dbContext);
                dbInitializer.LoadBasicInformationAsync().Wait();
                dbInitializer.LoadSampleDataAsync().Wait();
            }
            log.LogInformation("End write data...");
        }
    }
}
