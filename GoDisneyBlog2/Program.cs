using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GoDisneyBlog2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(SetUpConfiguration)
                .UseStartup<Startup>();


        private static void SetUpConfiguration(WebHostBuilderContext ctx, IConfigurationBuilder builder)
        {
            builder.Sources.Clear();

            builder.AddJsonFile("config.json", false, true)
                .AddEnvironmentVariables();
        }
    }
}
