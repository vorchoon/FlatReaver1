using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FlatReaver
{
    public class Program
    {
        /// <summary>
        /// Конфигурация из файла Config.json
        /// </summary>
        public static IConfiguration Configuration { get; protected set; }

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory).AddJsonFile(@"Config.json");
            Configuration = builder.Build();

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
