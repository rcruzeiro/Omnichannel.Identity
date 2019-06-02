using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Omnichannel.Identity.API
{
    public class Program
    {
        public static IHostingEnvironment HostingEnvironment { get; set; }

        public static IConfiguration Configuration { get; set; }

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    HostingEnvironment = hostingContext.HostingEnvironment;
                    config.SetBasePath(HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{HostingEnvironment.EnvironmentName}.json", true);
                    config.AddEnvironmentVariables();
                    Configuration = config.Build();
                })
                .UseStartup<Startup>();
    }
}
